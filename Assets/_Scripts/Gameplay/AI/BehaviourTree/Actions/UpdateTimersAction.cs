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
            float deltaTime = Time.deltaTime;

            float alertTimer = GetPinValue<float>(PinNames.ALERT_TIMER, contextRegistry);
            alertTimer += deltaTime;
            contextRegistry.SetVariableValue(PinNames.ALERT_TIMER, alertTimer);

            AlertLevel alertLevel = GetPinValue<AlertLevel>(PinNames.CURRENT_ALERT_LEVEL, contextRegistry);
            if (alertLevel == AlertLevel.Combat)
            {
                float combatTimer = GetPinValue<float>(PinNames.COMBAT_TIMER, contextRegistry);
                combatTimer += deltaTime;
                contextRegistry.SetVariableValue(PinNames.COMBAT_TIMER, combatTimer);
            }

            if (alertLevel == AlertLevel.Search)
            {
                float searchTime = GetPinValue<float>(PinNames.SEARCH_TIME_REMAINING, contextRegistry);
                searchTime -= deltaTime;
                searchTime = Mathf.Max(0.0f, searchTime);
                contextRegistry.SetVariableValue(PinNames.SEARCH_TIME_REMAINING, searchTime);
            }

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
            yield return OutputPin(PinNames.ALERT_TIMER, VariableType.Float);
            yield return OutputPin(PinNames.COMBAT_TIMER, VariableType.Float);
            yield return OutputPin(PinNames.SEARCH_TIME_REMAINING, VariableType.Float);
        }
    }
}