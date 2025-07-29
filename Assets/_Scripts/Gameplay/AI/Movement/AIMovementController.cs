using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Requests;
using Descent.Gameplay.Movement;
using UnityEngine;
using System;
using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using System.Collections.Generic;

namespace Descent.Gameplay.AI.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    [BehaviourTreeContextProvider(typeof(AIMovementContext))]
    [RequireComponent(typeof(BehaviourTreeActionRequestDispatcher))]
    public class AIMovementController : MonoBehaviour,
                                        IAIMovementController, IBehaviourTreeRequestReceiver, IBehaviourTreeContextProvider
    {
        private Rigidbody _rigidbody;
        private Vector3 _targetPosition = Vector3.zero;
        private bool _hasTarget = false;

        private BehaviourTreeActionRequestDispatcher _dispatcher;

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
            _dispatcher = GetComponent<BehaviourTreeActionRequestDispatcher>();

            _dispatcher.Register(BehaviourTreeActionType.MoveTo, this);
        }

        private void OnDestroy()
        {
            _dispatcher.Unregister(BehaviourTreeActionType.MoveTo);
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

            if (distance < _stoppingThreshold && IsMoving)
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