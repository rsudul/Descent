using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Requests;

namespace Descent.AI.BehaviourTree.Core
{
    public interface IBehaviourTreeAction
    {
        BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry);
        IBehaviourTreeAction Clone();
        void ResetAction();
        void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher);
    }
}