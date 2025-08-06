using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.Systems.Alert;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class SetAlertLevelAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher = null;

        [ShowInNodeInspector("Alert level")]
        [SerializeField]
        private AlertLevel _targetAlertLevel = AlertLevel.Idle;

        [ShowInNodeInspector("Reset timers")]
        [SerializeField]
        private bool _resetTimers = true;

        [ShowInNodeInspector("Search duration")]
        [SerializeField]
        private float _searchDuration = 10.0f;

        [ShowInNodeInspector("Cooldown duration")]
        [SerializeField]
        private float _cooldownDuration = 5.0f;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            SetAlertLevelActionData requestData = new SetAlertLevelActionData(_targetAlertLevel,
                _resetTimers, _searchDuration, _cooldownDuration);

            BehaviourTreeRequestResult result = _dispatcher.RequestAction(BehaviourTreeActionType.SetAlertLevel, requestData);

            if (result == BehaviourTreeRequestResult.Failure)
            {
                return BehaviourTreeStatus.Failure;
            }

            return BehaviourTreeStatus.Success;
        }

        public void ResetAction()
        {

        }

        public IBehaviourTreeAction Clone()
        {
            SetAlertLevelAction clone = new SetAlertLevelAction();
            clone._targetAlertLevel = _targetAlertLevel;
            clone._resetTimers = _resetTimers;
            clone._searchDuration = _searchDuration;
            clone._cooldownDuration = _cooldownDuration;
            return clone;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}