using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using Descent.Gameplay.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    public class RotateToTargetAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;
        private bool _requestedRotate = false;

        public float _targetYAngle = 0.0f;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.LogError("RotateToTargetAction does not reference a valid action request dispatcher.");
                return BehaviourTreeStatus.Failure;
            }

            AIRotationContext context = contextRegistry.GetContext(typeof(AIRotationContext)) as AIRotationContext;
            if (context == null || context.RotationController == null)
            {
                Debug.LogWarning("RotateToTargetAction: No AIRotationContext found.");
                return BehaviourTreeStatus.Failure;
            }

            if (!context.IsRotating && !_requestedRotate)
            {
                RotateToTargetActionData requestData = new RotateToTargetActionData(_targetYAngle);

                BehaviourTreeRequestResult result = _dispatcher.RequestAction(context.AgentTransform,
                    BehaviourTreeActionType.RotateTo, requestData);

                if (result == BehaviourTreeRequestResult.Failure)
                {
                    return BehaviourTreeStatus.Failure;
                }

                _requestedRotate = true;
            }

            return context.IsRotating ? BehaviourTreeStatus.Running : BehaviourTreeStatus.Success;
        }

        public IBehaviourTreeAction Clone()
        {
            RotateToTargetAction clone = new RotateToTargetAction();
            clone._targetYAngle = _targetYAngle;
            return clone;
        }

        public void ResetAction()
        {
            _requestedRotate = false;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}