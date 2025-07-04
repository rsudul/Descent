using Descent.Common.AI.BehaviourTree.Actions.Data;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Common.AI.BehaviourTree.Core.Requests;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Actions.Movement
{
    public class MoveToTargetAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.LogError("MoveToTargetAction does not reference a valid action request dispatcher.");
                return BehaviourTreeStatus.Failure;
            }

            AIMovementContext context = contextRegistry.GetContext(typeof(AIMovementContext)) as AIMovementContext;

            if (context == null)
            {
                Debug.LogWarning("MoveToTargetAction: No AIMovementContext found.");
                return BehaviourTreeStatus.Failure;
            }

            if (context.Agent == null || context.TargetPosition == null)
            {
                Debug.LogWarning("MoveToTargetAction: missing agent or target");
                return BehaviourTreeStatus.Failure;
            }

            var requestData = new MoveToActionData(context.TargetPosition.Value);

            BehaviourTreeRequestResult result = _dispatcher.RequestAction(context.AgentTransform,
                BehaviourTreeActionType.MoveTo,
                requestData);

            if (result == BehaviourTreeRequestResult.Failure)
            {
                return BehaviourTreeStatus.Failure;
            }

            return context.IsMoving ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            return new MoveToTargetAction();
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}