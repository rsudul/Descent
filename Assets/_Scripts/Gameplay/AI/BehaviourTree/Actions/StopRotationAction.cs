using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    public class StopRotationAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher = null;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.LogWarning("StopRotationAction: dispatcher not set.");
                return BehaviourTreeStatus.Failure;
            }

            AIRotationContext context = contextRegistry.GetContext(typeof(AIRotationContext)) as AIRotationContext;
            if (context == null || context.RotationController == null)
            {
                Debug.LogWarning("StopRotationAction: No AIRotationContext found.");
                return BehaviourTreeStatus.Failure;
            }

            BehaviourTreeRequestResult result = _dispatcher.RequestAction(context.AgentTransform,
                BehaviourTreeActionType.StopRotation, null);

            if (result == BehaviourTreeRequestResult.Failure)
            {
                return BehaviourTreeStatus.Failure;
            }

            if (result == BehaviourTreeRequestResult.Ignored)
            {
                return BehaviourTreeStatus.Failure;
            }

            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            return new StopRotationAction();
        }

        public void ResetAction()
        {

        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}