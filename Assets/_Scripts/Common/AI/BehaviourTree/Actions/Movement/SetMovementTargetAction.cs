using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Common.AI.BehaviourTree.Core.Requests;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Actions.Movement
{
    [System.Serializable]
    public class SetMovementTargetAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;

        [SerializeField]
        private Vector3 _targetPosition = Vector3.zero;

        public SetMovementTargetAction(Vector3 target)
        {
            _targetPosition = target;
        }

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            AIMovementContext context = contextRegistry.GetContext(typeof(AIMovementContext)) as AIMovementContext;

            if (context == null)
            {
                Debug.LogWarning("SetMovementTargetAction: No AIMovementContext found.");
                return BehaviourTreeStatus.Failure;
            }

            context.TargetPosition = _targetPosition;
            return BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            return new SetMovementTargetAction(_targetPosition);
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}