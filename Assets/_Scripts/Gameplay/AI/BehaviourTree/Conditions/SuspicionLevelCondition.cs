using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Variables;
using Descent.Common.Attributes.AI;
using Descent.Constants;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class SuspicionLevelCondition : BehaviourTreeConditionWithPins
    {
        [ShowInNodeInspector("Threshold")]
        [SerializeField]
        private float _threshold = 1.0f;

        public override bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            if (!HasPinConnection(PinNames.SUSPICION_LEVEL, contextRegistry))
            {
                return false;
            }

            float suspicionLevel = GetPinValue<float>(PinNames.SUSPICION_LEVEL, contextRegistry);
            return suspicionLevel >= _threshold;
        }

        public override IBehaviourTreeCondition Clone()
        {
            SuspicionLevelCondition clone = new SuspicionLevelCondition();
            clone._threshold = _threshold;
            return clone;
        }

        public override IEnumerable<ValuePinDefinition> GetRequiredPins()
        {
            yield return InputPin(PinNames.SUSPICION_LEVEL, VariableType.Float);
        }
    }
}