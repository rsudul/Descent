using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class AlertLevelCondition : IBehaviourTreeCondition
    {
        [ShowInNodeInspector("Alert level")]
        [SerializeField]
        private int _requiredAlertLevel = 0;

        [ShowInNodeInspector("Comparison")]
        [SerializeField]
        private BehaviourTreeCompareOperation _compareOperation = BehaviourTreeCompareOperation.Equal;

        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            int currentLevel = contextRegistry.Blackboard.Get<int>("AlertLevel", 0);

            return _compareOperation switch
            {
                BehaviourTreeCompareOperation.Equal => currentLevel == _requiredAlertLevel,
                BehaviourTreeCompareOperation.NotEqual => currentLevel != _requiredAlertLevel,
                BehaviourTreeCompareOperation.Less => currentLevel < _requiredAlertLevel,
                BehaviourTreeCompareOperation.LessOrEqual => currentLevel <= _requiredAlertLevel,
                BehaviourTreeCompareOperation.Greater => currentLevel > _requiredAlertLevel,
                BehaviourTreeCompareOperation.GreaterOrEqual => currentLevel >= _requiredAlertLevel,
                _ => false
            };
        }

        public void ResetCondition()
        {

        }

        public IBehaviourTreeCondition Clone()
        {
            AlertLevelCondition clone = new AlertLevelCondition();
            clone._requiredAlertLevel = _requiredAlertLevel;
            clone._compareOperation = _compareOperation;
            return clone;
        }
    }
}