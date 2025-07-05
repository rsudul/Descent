using Descent.Common.AI.BehaviourTree.Context;

namespace Descent.Common.AI.BehaviourTree.Conditions
{
    public interface IBehaviourTreeCondition
    {
        bool Check(BehaviourTreeContextRegistry contextRegistry);
        IBehaviourTreeCondition Clone();
    }
}