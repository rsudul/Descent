using System.Collections.Generic;
using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Variables;
using Descent.Constants;
using Descent.Gameplay.AI.BehaviourTree.Context;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class StoreLastKnownPositionAction : BehaviourTreeActionWithPins
    {
        public override BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;
            if (perceptionContext == null)
            {
                Debug.LogError("[StoreLastKnownPositionAction]: No AIPerceptionContext found.");
                return BehaviourTreeStatus.Failure;
            }

            if (perceptionContext.CurrentTarget == null)
            {
                contextRegistry.SetVariableValue(PinNames.HAS_ACTIVE_TARGET, false);
                return BehaviourTreeStatus.Success;
            }

            Vector3 targetPosition = perceptionContext.CurrentTarget.position;
            contextRegistry.SetVariableValue(PinNames.LAST_KNOWN_POSITION, targetPosition);
            contextRegistry.SetVariableValue(PinNames.HAS_ACTIVE_TARGET, true);

            return BehaviourTreeStatus.Success;
        }

        public override IBehaviourTreeAction Clone()
        {
            return new StoreLastKnownPositionAction();
        }

        public override IEnumerable<ValuePinDefinition> GetRequiredPins()
        {
            yield return OutputPin(PinNames.LAST_KNOWN_POSITION, VariableType.Vector3);
            yield return OutputPin(PinNames.HAS_ACTIVE_TARGET, VariableType.Bool);
        }
    }
}