using Descent.Common.AI.BehaviourTree.Core.Context;

namespace Descent.Common.AI.BehaviourTree.Core
{
    public interface IBehaviourTreeCondition
    {
        bool Check(BehaviourTreeContext context);
        IBehaviourTreeCondition Clone();
    }
}