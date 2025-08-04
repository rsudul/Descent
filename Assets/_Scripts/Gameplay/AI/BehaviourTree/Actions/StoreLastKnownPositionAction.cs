using UnityEngine;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public class StoreLastKnownPositionAction : IBehaviourTreeAction
    {
        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;
            if (perceptionContext == null)
            {
                Debug.LogError("[StoreLastKnownPositionAction]: No AIPerceptionContext found.");
                return BehaviourTreeStatus.Failure;
            }

            if (perceptionContext.CurrentTarget == null)
            {
                contextRegistry.Blackboard.Set("HasActiveTarget", false);
                return BehaviourTreeStatus.Success;
            }

            Vector3 targetPosition = perceptionContext.CurrentTarget.position;
            contextRegistry.Blackboard.Set("LastKnownTargetPosition", targetPosition);
            contextRegistry.Blackboard.Set("HasActiveTarget", true);

            return BehaviourTreeStatus.Success;
        }

        public void ResetAction()
        {

        }

        public IBehaviourTreeAction Clone()
        {
            return new StoreLastKnownPositionAction();
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {

        }
    }
}