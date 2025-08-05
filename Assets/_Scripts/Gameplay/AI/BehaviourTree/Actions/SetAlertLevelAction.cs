using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.AI.BehaviourTree.Variables;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.Systems.Alert;
using Descent.Constants;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class SetAlertLevelAction : BehaviourTreeActionWithPins
    {
        [ShowInNodeInspector("Alert level")]
        [SerializeField]
        private AlertLevel _targetAlertLevel = AlertLevel.Idle;

        [ShowInNodeInspector("Reset timers")]
        [SerializeField]
        private bool _resetTimers = true;

        public override BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            return BehaviourTreeStatus.Success;
        }

        public override void ResetAction()
        {

        }

        public override IBehaviourTreeAction Clone()
        {
            SetAlertLevelAction clone = new SetAlertLevelAction();
            clone._targetAlertLevel = _targetAlertLevel;
            clone._resetTimers = _resetTimers;
            return clone;
        }

        public override void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }

        public override IEnumerable<ValuePinDefinition> GetRequiredPins()
        {
            if (_resetTimers)
            {
                yield return InputPin(PinNames.SEARCH_DURATION, VariableType.Float);
                yield return InputPin(PinNames.COOLDOWN_DURATION, VariableType.Float);
            }
        }
    }
}