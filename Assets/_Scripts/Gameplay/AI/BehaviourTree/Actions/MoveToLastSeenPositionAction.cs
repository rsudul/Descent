using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;

namespace Descent.Gameplay.AI.BehaviourTree.Actions
{
    /// <summary>
    /// ---Move To Last Seen Position---
    /// 
    /// Action node which gets last seen position of a target from Blackboard,
    /// and sends a request to rotate towards that position.
    /// Returns Running while rotation is in progress, and Success after rotation is complete.
    /// </summary>
    public class MoveToLastSeenPositionAction : IBehaviourTreeAction
    {
        private BehaviourTreeActionRequestDispatcher _dispatcher;
        private bool _requested;
        private float _targetYAngle;

        [ShowInNodeInspector("Last seen key")]
        private string _lastSeenKey;
        [ShowInNodeInspector("Angle threshold")]
        private float _angleThreshold;

        public BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry)
        {
            if (_dispatcher == null)
            {
                Debug.LogError("[MoveToLastSeenPositionAction: dispatcher not set.");
                return BehaviourTreeStatus.Failure;
            }

            Vector3 lastSeenPosition = contextRegistry.Blackboard.Get<Vector3>(_lastSeenKey, Vector3.positiveInfinity);
            if (lastSeenPosition == Vector3.positiveInfinity)
            {
                Debug.LogWarning($"[MoveToLastSeenPosition]: no '{_lastSeenKey}' in Blackboard.");
                return BehaviourTreeStatus.Failure;
            }

            AIRotationContext rotationContext = contextRegistry.GetContext(typeof(AIRotationContext)) as AIRotationContext;
            if (rotationContext == null)
            {
                Debug.LogWarning("[MoveToLastSeenPosition]: no AIRotationContext found.");
                return BehaviourTreeStatus.Failure;
            }

            Vector3 direction = lastSeenPosition - rotationContext.AgentTransform.position;
            _targetYAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (!_requested && !rotationContext.IsRotating)
            {
                RotateToTargetActionData data = new RotateToTargetActionData(_targetYAngle);
                BehaviourTreeRequestResult result = _dispatcher.RequestAction(Descent.AI.BehaviourTree.Actions.BehaviourTreeActionType.RotateTo,
                                                                              data);

                if (result == BehaviourTreeRequestResult.Failure)
                {
                    Debug.LogWarning("[MoveToLastSeenPosition]: rotate request failed.");
                    return BehaviourTreeStatus.Failure;
                }

                _requested = true;
                return BehaviourTreeStatus.Running;
            }

            if (rotationContext.IsRotating)
            {
                return BehaviourTreeStatus.Running;
            }

            _requested = false;
            float currentY = rotationContext.RotationController.CurrentYAngle;
            float delta = Mathf.DeltaAngle(currentY, _targetYAngle);
            if (Mathf.Abs(delta) <= _angleThreshold)
            {
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Running;
        }

        public IBehaviourTreeAction Clone()
        {
            MoveToLastSeenPositionAction clone = new MoveToLastSeenPositionAction();
            clone._lastSeenKey = _lastSeenKey;
            clone._angleThreshold = _angleThreshold;
            return clone;
        }

        public void ResetAction()
        {
            _requested = false;
        }

        public void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}