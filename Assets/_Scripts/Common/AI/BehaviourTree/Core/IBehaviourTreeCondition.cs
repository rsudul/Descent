namespace Descent.Common.AI.BehaviourTree.Core
{
    public interface IBehaviourTreeCondition
    {
        bool Check(BehaviourTreeContext context);
    }
}