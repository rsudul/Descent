using Descent.Common.AI.BehaviourTree.Core.Context;

namespace Descent.Common.AI.BehaviourTree.Core
{
    public interface IBehaviourTreeCondition
    {
        bool Check(BehaviourTreeContextRegistry contextRegistry);
        IBehaviourTreeCondition Clone();
    }
}