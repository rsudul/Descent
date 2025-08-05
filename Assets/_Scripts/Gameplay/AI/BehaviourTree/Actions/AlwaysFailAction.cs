using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class AlwaysFailAction : IBehaviourTreeAction
    {
        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            return BehaviourTreeStatus.Failure;
        }

        public void ResetAction()
        {

        }

        public IBehaviourTreeAction Clone()
        {
            return new AlwaysFailAction();
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}