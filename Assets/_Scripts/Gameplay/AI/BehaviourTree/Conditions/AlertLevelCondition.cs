using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.Systems.Alert;
using Descent.Gameplay.AI.BehaviourTree.Context;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class AlertLevelCondition : IBehaviourTreeCondition
    {
        [ShowInNodeInspector("Alert level to check")]
        [SerializeField]
        private AlertLevel _targetAlertLevel = AlertLevel.Idle;

        [ShowInNodeInspector("Comparison")]
        [SerializeField]
        private BehaviourTreeCompareOperation _compareOperation = BehaviourTreeCompareOperation.Equal;

        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            AIAlertContext alertContext = contextRegistry.GetContext(typeof(AIAlertContext)) as AIAlertContext;

            if (alertContext == null)
            {
                return false;
            }

            AlertLevel currentLevel = alertContext.CurrentAlertLevel;

            bool result = _compareOperation switch
            {
                BehaviourTreeCompareOperation.Equal => currentLevel == _targetAlertLevel,
                BehaviourTreeCompareOperation.NotEqual => currentLevel != _targetAlertLevel,
                BehaviourTreeCompareOperation.Less => (int)currentLevel < (int)_targetAlertLevel,
                BehaviourTreeCompareOperation.LessOrEqual => (int)currentLevel <= (int)_targetAlertLevel,
                BehaviourTreeCompareOperation.Greater => (int)currentLevel > (int)_targetAlertLevel,
                BehaviourTreeCompareOperation.GreaterOrEqual => (int)currentLevel >= (int)_targetAlertLevel,
                _ => false
            };

            //Debug.Log(result);
            return result;
        }

        public IBehaviourTreeCondition Clone()
        {
            AlertLevelCondition clone = new AlertLevelCondition();
            clone._targetAlertLevel = _targetAlertLevel;
            clone._compareOperation = _compareOperation;
            return clone;
        }

        public void ResetCondition()
        {

        }
    }
}