using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.Systems.Alert;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class EscalateAlertAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher = null;

        [Header("Escalation mode")]
        [ShowInNodeInspector("Mode")]
        [SerializeField]
        private EscalationMode _mode = EscalationMode.Automatic;
        [ShowInNodeInspector("Target alert level")]
        [SerializeField]
        private AlertLevel _targetLevel = AlertLevel.Suspicious;

        [Header("Debug")]
        [ShowInNodeInspector("Log escalation")]
        private bool _logEscalation = true;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            AIAlertContext alertContext = contextRegistry.GetContext(typeof(AIAlertContext)) as AIAlertContext;
            if (alertContext == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            AlertLevel currentLevel = alertContext.CurrentAlertLevel;
            AlertLevel targetLevel = AlertLevel.Idle;

            switch (_mode)
            {
                case EscalationMode.Automatic:
                case EscalationMode.OneStep:
                    targetLevel = GetNextAlertLevel(currentLevel);
                    break;

                case EscalationMode.ToSpecificLevel:
                    targetLevel = _targetLevel;
                    break;

                default:
                    return BehaviourTreeStatus.Failure;
            }

            if (targetLevel == currentLevel)
            {
                return BehaviourTreeStatus.Success;
            }

            EscalateAlertActionData escalateData = new EscalateAlertActionData(targetLevel);
            BehaviourTreeRequestResult result = _dispatcher.RequestAction(BehaviourTreeActionType.EscalateAlert, escalateData);

            if (_logEscalation && result == BehaviourTreeRequestResult.Success)
            {
                Debug.Log($"[EscalateAlertAction] Escalated from {currentLevel} to {targetLevel}");
            }

            return result == BehaviourTreeRequestResult.Success ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }

        public IBehaviourTreeAction Clone()
        {
            EscalateAlertAction clone = new EscalateAlertAction();
            clone._mode = _mode;
            clone._targetLevel = _targetLevel;
            return clone;
        }

        public void ResetAction()
        {

        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        private AlertLevel GetNextAlertLevel(AlertLevel current)
        {
            return current switch
            {
                AlertLevel.Idle => AlertLevel.Suspicious,
                AlertLevel.Suspicious => AlertLevel.Alert,
                AlertLevel.Alert => AlertLevel.Combat,
                AlertLevel.Combat => AlertLevel.Combat,
                AlertLevel.Search => AlertLevel.Combat,
                AlertLevel.Cooldown => AlertLevel.Suspicious,
                _ => AlertLevel.Suspicious
            };
        }
    }
}