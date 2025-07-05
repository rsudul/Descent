using Descent.Common.AI.BehaviourTree.Context;
using Descent.Common.AI.BehaviourTree.Requests;

namespace Descent.Common.AI.BehaviourTree.Core
{
    public interface IBehaviourTreeAction
    {
        BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry);
        IBehaviourTreeAction Clone();
        void ResetAction();
        void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher);
    }
}