using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    public class UpdateTargetMemoryAction : IBehaviourTreeAction
    {
        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;
            if (perceptionContext == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            if (perceptionContext.CurrentTarget == null)
            {
                contextRegistry.Blackboard.Set("HasActiveTarget", false);
            }
            else
            {
                contextRegistry.Blackboard.Set("LastKnownPosition", perceptionContext.CurrentTarget.position);
                contextRegistry.Blackboard.Set("HasActiveTarget", true);
            }

            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            return new UpdateTargetMemoryAction();
        }

        public void ResetAction()
        {

        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}