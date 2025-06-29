using Descent.Common.AI.BehaviourTree.Actions.Data;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Common.AI.BehaviourTree.Core.Requests;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Actions.Movement
{
    public class MoveToTargetAction : IBehaviourTreeAction
    {
        private readonly float _speed;

        public MoveToTargetAction(float speed = 5.0f)
        {
            _speed = speed;
        }

        public BehaviourTreeStatus Execute(BehaviourTreeContext context)
        {
            if (context is not AIMovementContext movementContext)
            {
                Debug.LogWarning("MoveToTargetAction: context is not AIMovementContext");
                return BehaviourTreeStatus.Failure;
            }

            if (movementContext.Agent == null || movementContext.TargetPosition == null)
            {
                Debug.LogWarning("MoveToTargetAction: missing agent or target");
                return BehaviourTreeStatus.Failure;
            }

            var requestData = new MoveToActionData(movementContext.TargetPosition.Value, 5.0f);
            var result = BehaviourTreeActionRequestDispatcher.Instance?.RequestAction(
                movementContext.AgentTransform,
                "MoveTo",
                requestData
             );

            if (result == BehaviourTreeRequestResult.Failure)
            {
                return BehaviourTreeStatus.Failure;
            }

            return movementContext.IsMoving ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            return new MoveToTargetAction(_speed);
        }
    }
}