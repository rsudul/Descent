using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Variables;
using Descent.Common.Attributes.AI;
using Descent.Constants;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class UpdateSuspicionAction : BehaviourTreeActionWithPins
    {
        [ShowInNodeInspector("Increase rate")]
        [SerializeField]
        private float _increaseRate = 1.0f;

        [ShowInNodeInspector("Use decay")]
        [SerializeField]
        private bool _useDecay = true;

        public override BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            float currentSuspicion = GetPinValue<float>(PinNames.SUSPICION_LEVEL, contextRegistry);
            bool hasTarget = GetPinValue<bool>(PinNames.HAS_ACTIVE_TARGET, contextRegistry);

            if (hasTarget)
            {
                currentSuspicion += _increaseRate * Time.deltaTime;
                currentSuspicion = Mathf.Clamp01(currentSuspicion);
            }
            else if (_useDecay)
            {
                float decayRate = GetPinValue<float>(PinNames.SUSPICION_DECAY_RATE, contextRegistry);
                currentSuspicion -= decayRate * Time.deltaTime;
                currentSuspicion = Mathf.Max(0.0f, currentSuspicion);
            }

            return BehaviourTreeStatus.Success;
        }

        public override IBehaviourTreeAction Clone()
        {
            UpdateSuspicionAction clone = new UpdateSuspicionAction();
            clone._increaseRate = _increaseRate;
            clone._useDecay = _useDecay;
            return clone;
        }

        public override IEnumerable<ValuePinDefinition> GetRequiredPins()
        {
            yield return InputPin(PinNames.HAS_ACTIVE_TARGET, VariableType.Bool);
            yield return InputPin(PinNames.SUSPICION_LEVEL, VariableType.Float);
            yield return InputPin(PinNames.SUSPICION_THRESHOLD, VariableType.Float);
            yield return InputPin(PinNames.SUSPICION_DECAY_RATE, VariableType.Float);
        }
    }
}