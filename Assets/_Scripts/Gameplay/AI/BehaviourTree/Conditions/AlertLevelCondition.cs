using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Variables;
using Descent.Common.Attributes.AI;
using Descent.Constants;
using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class AlertLevelCondition : BehaviourTreeConditionWithPins
    {
        [ShowInNodeInspector("Alert level to check")]
        [SerializeField]
        private AlertLevel _targetAlertLevel = AlertLevel.Idle;

        [ShowInNodeInspector("Comparison")]
        [SerializeField]
        private BehaviourTreeCompareOperation _compareOperation = BehaviourTreeCompareOperation.Equal;

        public override bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            AlertLevel currentLevel = GetPinValue<AlertLevel>(PinNames.CURRENT_ALERT_LEVEL, contextRegistry);

            return _compareOperation switch
            {
                BehaviourTreeCompareOperation.Equal => currentLevel == _targetAlertLevel,
                BehaviourTreeCompareOperation.NotEqual => currentLevel != _targetAlertLevel,
                BehaviourTreeCompareOperation.Less => (int)currentLevel < (int)_targetAlertLevel,
                BehaviourTreeCompareOperation.LessOrEqual => (int)currentLevel <= (int)_targetAlertLevel,
                BehaviourTreeCompareOperation.Greater => (int)currentLevel > (int)_targetAlertLevel,
                BehaviourTreeCompareOperation.GreaterOrEqual => (int)currentLevel >= (int)_targetAlertLevel,
                _ => false
            };
        }

        public override IBehaviourTreeCondition Clone()
        {
            AlertLevelCondition clone = new AlertLevelCondition();
            clone._targetAlertLevel = _targetAlertLevel;
            clone._compareOperation = _compareOperation;
            return clone;
        }

        public override IEnumerable<ValuePinDefinition> GetRequiredPins()
        {
            yield return InputPin(PinNames.CURRENT_ALERT_LEVEL, VariableType.Enum);
        }
    }
}