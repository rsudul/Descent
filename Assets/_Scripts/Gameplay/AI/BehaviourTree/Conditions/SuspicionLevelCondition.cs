using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.Common.Attributes.AI;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class SuspicionLevelCondition : IBehaviourTreeCondition
    {
        [ShowInNodeInspector("Threshold")]
        [SerializeField]
        private float _threshold = 1.0f;

        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            float suspicionLevel = contextRegistry.Blackboard.Get<float>("SuspicionLevel", 0.0f);
            return suspicionLevel >= _threshold;
        }

        public IBehaviourTreeCondition Clone()
        {
            SuspicionLevelCondition clone = new SuspicionLevelCondition();
            clone._threshold = _threshold;
            return clone;
        }

        public void ResetCondition()
        {

        }
    }
}