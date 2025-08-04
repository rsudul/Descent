using Descent.AI.BehaviourTree.Core;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Actions
{
    public interface IBehaviourTreeActionWithPins : IBehaviourTreeAction
    {
        IEnumerable<ValuePinDefinition> GetRequiredPins();
        void SetNodeGuid(string guid);
    }
}