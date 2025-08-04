using Descent.AI.BehaviourTree.Core;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Conditions
{
    public interface IBehaviourTreeConditionWithPins : IBehaviourTreeCondition
    {
        IEnumerable<ValuePinDefinition> GetRequiredPins();
        void SetNodeGuid(string guid);
    }
}