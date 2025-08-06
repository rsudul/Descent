using System;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.Systems.Alert;
using Descent.Gameplay.Events.Arguments;

namespace Descent.Gameplay.AI.Alert
{
    [BehaviourTreeContextProvider(typeof(AIAlertContext))]
    public class AIAlertController : MonoBehaviour,
                                     IAlertController,
                                     IBehaviourTreeRequestReceiver,
                                     IBehaviourTreeContextProvider
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;

        private AlertLevel _currentAlertLevel = AlertLevel.Idle;
        private float _alertTimer = 0.0f;
        private float _combatTimer = 0.0f;
        private float _searchTimeRemaining = 0.0f;

        private float _searchDuration = 0.0f;
        private float _cooldownDuration = 0.0f;

        public AlertLevel CurrentAlertLevel => _currentAlertLevel;
        public float AlertTimer => _alertTimer;
        public float CombatTimer => _combatTimer;
        public float SearchTimeRemaining => _searchTimeRemaining;

        public event EventHandler<AlertLevelChangedEventArgs> OnAlertLevelChanged;

        [Header("Alert settings")]
        [SerializeField]
        private AlertLevel _startingAlertLevel = AlertLevel.Idle;
        [SerializeField]
        private float _defaultSearchDuration = 15.0f;
        [SerializeField]
        private float _defaultCooldownDuration = 10.0f;

        [Header("Timer settings")]
        [SerializeField]
        private float _alertDecayRate = 1.0f;
        [SerializeField]
        private float _combatTimeoutDuration = 8.0f;

        private void Awake()
        {
            _dispatcher = GetComponent<BehaviourTreeActionRequestDispatcher>();
            if (_dispatcher != null)
            {
                _dispatcher.Register(BehaviourTreeActionType.SetAlertLevel, this);
            }

            _currentAlertLevel = _startingAlertLevel;
            _searchDuration = _defaultSearchDuration;
            _cooldownDuration += _defaultCooldownDuration;

            ResetTimers();
        }

        private void Update()
        {
            UpdateTimers(Time.deltaTime);
        }

        private void OnDestroy()
        {
            if (_dispatcher != null)
            {
                _dispatcher.Unregister(BehaviourTreeActionType.SetAlertLevel);
            }
        }

        public void SetAlertLevel(AlertLevel alertLevel)
        {
            if (_currentAlertLevel == alertLevel)
            {
                return;
            }

            AlertLevel previousLevel = _currentAlertLevel;
            _currentAlertLevel = alertLevel;

            float timeSinceLastChange = _alertTimer;

            AlertLevelChangedEventArgs args = new AlertLevelChangedEventArgs(previousLevel, _currentAlertLevel, timeSinceLastChange);
            OnAlertLevelChanged?.Invoke(this, args);

            HandleAlertLevelChanged(previousLevel, _currentAlertLevel);
        }

        public void UpdateTimers(float deltaTime)
        {
            _alertTimer += deltaTime;

            switch (_currentAlertLevel)
            {
                case AlertLevel.Combat:
                    _combatTimer += deltaTime;
                    if (_combatTimer > _combatTimeoutDuration)
                    {
                        SetAlertLevel(AlertLevel.Search);
                    }
                    break;

                case AlertLevel.Search:
                    _searchTimeRemaining = Mathf.Max(0.0f, _searchTimeRemaining - deltaTime);
                    if (_searchTimeRemaining <= 0.0f)
                    {
                        SetAlertLevel(AlertLevel.Cooldown);
                    }
                    break;

                case AlertLevel.Cooldown:
                    if (_alertTimer > _cooldownDuration)
                    {
                        SetAlertLevel(AlertLevel.Idle);
                    }
                    break;

                case AlertLevel.Suspicious:
                case AlertLevel.Alert:
                    if (_alertTimer > _alertDecayRate)
                    {
                        DeescalateAlert();
                    }
                    break;
            }
        }

        public void ResetTimers()
        {
            _alertTimer = 0.0f;
            _combatTimer = 0.0f;
            _searchTimeRemaining = 0.0f;
        }

        public void SetSearchDuration(float duration)
        {
            _searchDuration = duration;

            if (_currentAlertLevel == AlertLevel.Search)
            {
                _searchTimeRemaining = duration;
            }
        }

        public void SetCooldownTimer(float duration)
        {
            _cooldownDuration = duration;
        }

        public bool IsInState(AlertLevel alertLevel)
        {
            return _currentAlertLevel == alertLevel;
        }

        public bool TimerExceedsThreshold(float threshold)
        {
            return _alertTimer > threshold;
        }

        public BehaviourTreeRequestResult HandleRequest(BehaviourTreeActionType actionType, IBehaviourTreeActionData data)
        {
            if (actionType != BehaviourTreeActionType.SetAlertLevel)
            {
                return BehaviourTreeRequestResult.Ignored;
            }

            if (data is not SetAlertLevelActionData alertData)
            {
                return BehaviourTreeRequestResult.Failure;
            }

            SetAlertLevel(alertData.TargetAlertLevel);

            if (alertData.ResetTimers)
            {
                ResetTimers();
                SetSearchDuration(alertData.SearchDuration);
                SetCooldownTimer(alertData.CooldownDuration);
            }

            return BehaviourTreeRequestResult.Success;
        }

        public BehaviourTreeContext GetBehaviourTreeContext(Type contextType, GameObject agent)
        {
            if (contextType == typeof(AIAlertContext))
            {
                return new AIAlertContext(agent, this);
            }

            return null;
        }

        private void HandleAlertLevelChanged(AlertLevel previous, AlertLevel current)
        {
            switch (current)
            {
                case AlertLevel.Combat:
                    _combatTimer = 0.0f;
                    break;

                case AlertLevel.Search:
                    _searchTimeRemaining = _searchDuration;
                    break;

                case AlertLevel.Cooldown:
                case AlertLevel.Idle:
                    ResetTimers();
                    break;
            }
        }

        private void DeescalateAlert()
        {
            switch (_currentAlertLevel)
            {
                case AlertLevel.Alert:
                    SetAlertLevel(AlertLevel.Suspicious);
                    break;

                case AlertLevel.Suspicious:
                    SetAlertLevel(AlertLevel.Idle);
                    break;
            }
        }

        public void EscalateAlert()
        {
            switch (_currentAlertLevel)
            {
                case AlertLevel.Idle:
                    SetAlertLevel(AlertLevel.Suspicious);
                    break;

                case AlertLevel.Suspicious:
                    SetAlertLevel(AlertLevel.Alert);
                    break;

                case AlertLevel.Alert:
                    SetAlertLevel(AlertLevel.Combat);
                    break;

                case AlertLevel.Search:
                    SetAlertLevel(AlertLevel.Combat);
                    break;
            }
        }

        [ContextMenu("Escalate Alert")]
        private void DebugEscalate() => EscalateAlert();

        [ContextMenu("Set to Combat")]
        private void DebugCombat() => SetAlertLevel(AlertLevel.Combat);

        [ContextMenu("Reset to Idle")]
        private void DebugReset() => SetAlertLevel(AlertLevel.Idle);

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            GUILayout.BeginArea(new Rect(10, 10, 250, 150));
            GUILayout.Label($"Alert level: {_currentAlertLevel}");
            GUILayout.Label($"Alert timer: {_alertTimer:F1}s");
            GUILayout.Label($"Combat timer: {_combatTimer:F1}s");
            GUILayout.Label($"Search remaining: {_searchTimeRemaining:F1}s");
            GUILayout.EndArea();
        }
    }
}