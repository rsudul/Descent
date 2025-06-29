using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Actions.Movement
{
    [System.Serializable]
    public class SetMovementTargetAction : IBehaviourTreeAction
    {
        [SerializeField]
        private Vector3 _targetPosition = Vector3.zero;

        public SetMovementTargetAction(Vector3 target)
        {
            _targetPosition = target;
        }

        public BehaviourTreeStatus Execute(BehaviourTreeContext context)
        {
            if (context is not AIMovementContext movementContext)
            {
                Debug.LogWarning("SetMovementTargetAction: context is not AIMovementContext.");
                return BehaviourTreeStatus.Failure;
            }

            movementContext.TargetPosition = _targetPosition;
            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            return new SetMovementTargetAction(_targetPosition);
        }
    }
}