using Descent.AI.BehaviourTree.Actions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Requests;
using Descent.Common.Attributes.AI;
using Descent.Gameplay.AI.BehaviourTree.Actions.Data;
using Descent.Gameplay.AI.BehaviourTree.Context;
using Descent.Gameplay.Movement;
using UnityEngine;

namespace Descent.Gameplay.AI.Movement
{
    [BehaviourTreeContextProvider(typeof(AIRotationContext))]
    public class AIRotationController : MonoBehaviour,
                                        IAIRotationController, IBehaviourTreeRequestReceiver, IBehaviourTreeContextProvider
    {
        private float _targetYAngle = 0.0f;
        private bool _hasTarget = false;

        private BehaviourTreeActionRequestDispatcher _dispatcher;

        public float RotationSpeed => _rotationSpeed;
        public bool IsRotating => _hasTarget;
        public float CurrentYAngle => _rotatingTransform.localEulerAngles.y;

        [SerializeField]
        private Transform _rotatingTransform = null;
        [SerializeField]
        private float _rotationSpeed = 120.0f;
        [SerializeField]
        private float _angleThreshold = 0.5f;

        private void Start()
        {
            _dispatcher = GetComponent<BehaviourTreeActionRequestDispatcher>();
            _dispatcher.Register(transform, this);
        }

        private void OnDestroy()
        {
            _dispatcher.Unregister(transform);
        }

        private void Update()
        {
            if (!_hasTarget)
            {
                return;
            }

            float currentY = _rotatingTransform.localEulerAngles.y;
            if (currentY > 180.0f)
            {
                currentY -= 360.0f;
            }
            float angle = Mathf.MoveTowardsAngle(currentY, _targetYAngle, Time.deltaTime * _rotationSpeed);

            _rotatingTransform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);

            if (Mathf.Abs(Mathf.DeltaAngle(angle, _targetYAngle)) < _angleThreshold)
            {
                _rotatingTransform.localRotation = Quaternion.Euler(0.0f, _targetYAngle, 0.0f);
                StopRotation();
            }
        }

        public void RotateTo(float targetYAngle)
        {
            _targetYAngle = targetYAngle;
            _hasTarget = true;
        }

        public void StopRotation()
        {
            _hasTarget = false;
        }

        public BehaviourTreeRequestResult HandleRequest(BehaviourTreeActionType actionType,
            IBehaviourTreeActionData actionData)
        {
            if (actionType != BehaviourTreeActionType.RotateTo
                && actionType != BehaviourTreeActionType.StopRotation)
            {
                return BehaviourTreeRequestResult.Ignored;
            }

            if (actionType == BehaviourTreeActionType.RotateTo
                && actionData != null && actionData is RotateToTargetActionData rotateData)
            {
                RotateTo(rotateData.TargetYAngle);
                return BehaviourTreeRequestResult.Success;
            }

            if (actionType == BehaviourTreeActionType.StopRotation)
            {
                StopRotation();
                return BehaviourTreeRequestResult.Success;
            }

            return BehaviourTreeRequestResult.Failure;
        }

        public BehaviourTreeContext GetBehaviourTreeContext(System.Type contextType, GameObject agent)
        {
            if (contextType == typeof(AIRotationContext))
            {
                return new AIRotationContext(agent, this);
            }

            return null;
        }
    }
}