using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.AI.BehaviourTree.Variables;
using Descent.Constants;
using System.Collections.Generic;
using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class UpdateTimersAction : BehaviourTreeActionWithPins
    {
        public override BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            return BehaviourTreeStatus.Success;
        }

        public override IBehaviourTreeAction Clone()
        {
            return new UpdateTimersAction();
        }

        public override IEnumerable<ValuePinDefinition> GetRequiredPins()
        {
            yield return InputPin(PinNames.CURRENT_ALERT_LEVEL, VariableType.Enum);
            yield return InputPin(PinNames.ALERT_TIMER, VariableType.Float);
            yield return InputPin(PinNames.COMBAT_TIMER, VariableType.Float);
            yield return InputPin(PinNames.SEARCH_TIME_REMAINING, VariableType.Float);
        }
    }
}