using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Actions;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    /// <summary>
    /// ---Predictive Aim---
    /// 
    /// Action node that aims at the target by predicting its future position based on velocity.
    /// It requests rotation via AIRotationController and returns Running until the turret is aligned within threshold.
    /// 
    /// Uses contexts:
    ///  - AIPerceptionContext,
    ///  - AIRotationContext
    /// </summary>
    public class PredictiveAimAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;
        private bool _requestedRotate = false;
        private float _targetYAngle = 0.0f;

        [SerializeField]
        [ShowInNodeInspector("Lead time")]
        private float _leadTime = 0.0f;
        [SerializeField]
        [ShowInNodeInspector("Angle threshold")]
        private float _angleThreshold = 0.0f;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.LogWarning("[PredictiveAimAction]: No BehaviourTreeActionRequestDispatcher found.");
                return BehaviourTreeStatus.Failure;
            }

            AIPerceptionContext perceptionContext = contextRegistry.GetContext(typeof(AIPerceptionContext)) as AIPerceptionContext;
            if (perceptionContext == null)
            {
                Debug.Log("[PredictiveAimAction]: No AIPerceptionContext found.");
                return BehaviourTreeStatus.Failure;
            }
            if (perceptionContext.PerceptionController == null)
            {
                Debug.Log("[PredictiveAimAction]: No PerceptionController found.");
                return BehaviourTreeStatus.Failure;
            }
            if (perceptionContext.CurrentTarget == null)
            {
                Debug.Log("[PredictiveAimTarget]: No current target.");
                return BehaviourTreeStatus.Failure;
            }

            AIRotationContext rotationContext = contextRegistry.GetContext(typeof(AIRotationContext)) as AIRotationContext;
            if (rotationContext == null)
            {
                Debug.Log("[PredictiveAimAction]: No AIRotationContext found.");
                return BehaviourTreeStatus.Failure;
            }
            if (rotationContext.RotationController == null)
            {
                Debug.Log("[PredictiveAimAction]: No RotationController found.");
                return BehaviourTreeStatus.Failure;
            }

            Vector3 targetPosition = perceptionContext.CurrentTarget.position;
            Vector3 velocity = Vector3.zero;
            Rigidbody rigidbody = perceptionContext.CurrentTarget.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                velocity = rigidbody.velocity;
            }
            Vector3 predictedPosition = targetPosition + velocity * _leadTime;

            Vector3 direction = predictedPosition - rotationContext.AgentTransform.position;
            _targetYAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float currentY = rotationContext.RotationController.CurrentYAngle;
            float delta = Mathf.DeltaAngle(currentY, _targetYAngle);
            if (Mathf.Abs(delta) <= _angleThreshold)
            {
                return BehaviourTreeStatus.Running;
            }

            if (!rotationContext.IsRotating && !_requestedRotate)
            {
                RotateToTargetActionData data = new RotateToTargetActionData(_targetYAngle);
                BehaviourTreeRequestResult result = _dispatcher.RequestAction(rotationContext.AgentTransform,
                                                                              BehaviourTreeActionType.RotateTo, data);
                if (result == BehaviourTreeRequestResult.Failure)
                {
                    return BehaviourTreeStatus.Failure;
                }
                _requestedRotate = true;
            }

            if (rotationContext.IsRotating)
            {
                return BehaviourTreeStatus.Running;
            }


            _requestedRotate = false;
            return BehaviourTreeStatus.Running;
        }

        public IBehaviourTreeAction Clone()
        {
            PredictiveAimAction clone = new PredictiveAimAction();
            clone._leadTime = _leadTime;
            clone._angleThreshold = _angleThreshold;
            return clone;
        }

        public void ResetAction()
        {
            _requestedRotate = false;
        }

        public void InjectDispatcher (BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}