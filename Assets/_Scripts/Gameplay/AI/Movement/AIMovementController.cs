using Descent.Attributes.AI;
using Descent.Common.AI.BehaviourTree.Actions.Data;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Common.AI.BehaviourTree.Core.Requests;
using Descent.Gameplay.Movement;
using UnityEngine;
using System;
using Descent.Common.AI.BehaviourTree.Actions;

namespace Descent.Gameplay.AI.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [BehaviourTreeContextProvider(typeof(AIMovementContext))]
    public class AIMovementController : MonoBehaviour,
                                        IMovementController, IBehaviourTreeRequestReceiver, IBehaviourTreeContextProvider
    {
        private Rigidbody _rigidbody;
        private Vector3 _targetPosition = Vector3.zero;
        private bool _hasTarget = false;

        public Vector3 Velocity { get; private set; }
        public bool IsMoving => _hasTarget;

        [SerializeField]
        private float _speed = 4.0f;
        [SerializeField]
        private float _acceleration = 8.0f;
        [SerializeField]
        private float _stoppingThreshold = 0.1f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            BehaviourTreeActionRequestDispatcher.Instance?.Register(transform, this);
        }

        private void OnDestroy()
        {
            BehaviourTreeActionRequestDispatcher.Instance?.Unregister(transform);
        }

        private void FixedUpdate()
        {
            if (!_hasTarget)
            {
                Velocity = Vector3.zero;
                return;
            }

            Vector3 direction = _targetPosition - _rigidbody.position;
            float distance = direction.magnitude;

            if (distance < _stoppingThreshold)
            {
                Stop();
                return;
            }

            Vector3 desiredVelocity = direction.normalized * _speed;
            Vector3 currentVelocity = _rigidbody.velocity;
            Vector3 newVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, _acceleration * Time.fixedDeltaTime);

            _rigidbody.velocity = newVelocity;
            Velocity = newVelocity;
        }

        public void MoveTo(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            _hasTarget = true;
        }

        public void Stop()
        {
            _hasTarget = false;
            Velocity = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
        }

        public BehaviourTreeRequestResult HandleRequest(BehaviourTreeActionType actionType, IBehaviourTreeActionData data)
        {
            if (actionType != BehaviourTreeActionType.MoveTo)
            {
                return BehaviourTreeRequestResult.Ignored;
            }

            if (data is not MoveToActionData actionData)
            {
                return BehaviourTreeRequestResult.Failure;
            }

            MoveTo(actionData.Target);
            return BehaviourTreeRequestResult.Success;
        }

        public BehaviourTreeContext GetBehaviourTreeContext(Type contextType, GameObject agent)
        {
            if (contextType == typeof(AIMovementContext))
            {
                return new AIMovementContext(agent, this);
            }

            return null;
        }
    }
}