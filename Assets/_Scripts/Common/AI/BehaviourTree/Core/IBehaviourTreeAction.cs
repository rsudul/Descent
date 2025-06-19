namespace Descent.Common.AI.BehaviourTree.Core
{
    public interface IBehaviourTreeAction
    {
        BehaviourTreeStatus Execute(BehaviourTreeContext context);
    }
}