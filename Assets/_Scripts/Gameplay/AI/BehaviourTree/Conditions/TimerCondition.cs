using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.Common.Attributes.AI;
using System.Collections.Generic;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class TimerCondition : IBehaviourTreeCondition
    {
        [ShowInNodeInspector("Timer key")]
        [SerializeField]
        private string _timerKey = "AlertTimer";

        [ShowInNodeInspector("Threshold")]
        [SerializeField]
        private float _threshold = 0.0f;

        [ShowInNodeInspector("Greater than")]
        [SerializeField]
        private bool _greaterThan = true;

        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            float timerValue = contextRegistry.Blackboard.Get<float>(_timerKey, 0.0f);
            return _greaterThan ? timerValue > _threshold : timerValue <= _threshold;
        }

        public void ResetCondition()
        {

        }

        public IBehaviourTreeCondition Clone()
        {
            TimerCondition clone = new TimerCondition();
            clone._timerKey = _timerKey;
            clone._threshold = _threshold;
            clone._greaterThan = _greaterThan;
            return clone;
        }
    }
}