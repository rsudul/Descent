using Descent.Common.AI.BehaviourTree.Actions.Data;
using Descent.Common.AI.BehaviourTree.Core.Requests;
using Descent.Gameplay.Movement;
using UnityEngine;

namespace Descent.Gameplay.AI.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class AIMovementController : MonoBehaviour, IMovementController, IBehaviourTreeRequestReceiver
    {
        private Rigidbody _rigidbody;
        private Vector3 _targetPosition = Vector3.zero;
        private float _speed = 4.0f;
        private bool _hasTarget = false;

        public Vector3 Velocity { get; private set; }
        public bool IsMoving => _hasTarget;

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

        public void MoveTo(Vector3 targetPosition, float speed)
        {
            _targetPosition = targetPosition;
            _speed = speed;
            _hasTarget = true;
        }

        public void Stop()
        {
            _hasTarget = false;
            Velocity = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
        }

        public BehaviourTreeRequestResult HandleRequest(string actionType, IBehaviourTreeActionData data)
        {
            if (actionType != "MoveTo")
            {
                return BehaviourTreeRequestResult.Ignored;
            }

            if (data is not MoveToActionData actionData)
            {
                return BehaviourTreeRequestResult.Failure;
            }

            MoveTo(actionData.Target, actionData.Speed);
            return BehaviourTreeRequestResult.Success;
        }
    }
}