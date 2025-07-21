using Descent.AI.BehaviourTree.Context;

namespace Descent.AI.BehaviourTree.Conditions
{
    public interface IBehaviourTreeCondition
    {
        bool Check(BehaviourTreeContextRegistry contextRegistry);
        IBehaviourTreeCondition Clone();
        void ResetCondition();
    }
}