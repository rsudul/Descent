using Descent.Extensions.Math;
using Descent.Gameplay.Player.Settings.Movement;
using UnityEngine;

namespace Descent.Gameplay.Player.Movement
{
    [System.Serializable]
    public class PlayerMovementController
    {
        private float _pitchRotation = 0.0f;
        private float _yawRotation = 0.0f;
        private float _rollRotation = 0.0f;
        private Quaternion _rotation = Quaternion.identity;
        private float _rollStabilizationTimer = 0.0f;
        private float _rollStabilizationDelay = 0.5f;
        private bool _isStabilizingAxis = false;

        private Vector3 _movementDirection = Vector3.zero;
        private Vector3 _lastVelocity = Vector3.zero;

        private float _movementFreezeTimer = 0.0f;
        private float _bounceRotationFreezeTimer = 0.0f;

        public Vector3 LastVelocity => _lastVelocity;

        [Header("Settings")]
        [SerializeField]
        private PlayerMovementSettings _movementSettings;

        public void Initialize(Transform transform, Rigidbody rigidbody)
        {
            _rotation = transform.rotation;
            InitializeRigidbody(rigidbody);
        }

        private void InitializeRigidbody(Rigidbody rigidbody)
        {
            rigidbody.freezeRotation = true;
            rigidbody.useGravity = false;
            rigidbody.drag = 0.0f;
            rigidbody.angularDrag = 0.0f;
            rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.isKinematic = false;
        }

        public void UpdateLook(Transform transform, Rigidbody rigidbody, float deltaTime)
        {
            bool isLookingNow = _pitchRotation != 0.0f || _yawRotation != 0.0f || _rollRotation != 0.0f;

            if (isLookingNow)
            {
                _isStabilizingAxis = false;
                _rollStabilizationTimer = _rollStabilizationDelay;

                _rotation = rigidbody.rotation;
                _rotation *= Quaternion.AngleAxis(-_yawRotation, Vector3.right);
                _rotation *= Quaternion.AngleAxis(_pitchRotation, Vector3.up);
                _rotation *= Quaternion.AngleAxis(_rollRotation, Vector3.forward);
            }
            else
            {
                _rollStabilizationTimer -= deltaTime;

                if (_rollStabilizationTimer <= 0.0f)
                {
                    _isStabilizingAxis = true;
                    StabilizeRollAxis(transform, rigidbody, deltaTime, out bool axisStabilized);

                    if (axisStabilized)
                    {
                        _rollStabilizationTimer = _rollStabilizationDelay;
                    }
                }
            }

            float rotationSpeed = _isStabilizingAxis ? _movementSettings.RollAxisResetSpeed : _movementSettings.LookSmoothness;
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, _rotation, deltaTime * rotationSpeed);
        }

        public void UpdateMovement(Transform transform, Rigidbody rigidbody, float deltaTime)
        {
            if (_movementFreezeTimer > 0.0f)
            {
                _movementFreezeTimer -= deltaTime;
                return;
            }
            else
            {
                _movementFreezeTimer = 0.0f;
            }

            Vector3 currentVelocity = rigidbody.velocity;

            _lastVelocity = currentVelocity;

            Vector3 targetVelocity = rigidbody.rotation * _movementDirection;

            targetVelocity = targetVelocity * _movementSettings.MovementSpeed;

            float speedChangeX = _movementSettings.Acceleration;
            float speedChangeY = _movementSettings.Acceleration;
            float speedChangeZ = _movementSettings.Acceleration;

            Vector3 relativeCurrentVelocity = transform.InverseTransformDirection(currentVelocity);
            Vector3 relativeTargetVelocity = transform.InverseTransformDirection(targetVelocity);

            if (relativeTargetVelocity.x == 0.0f)
            {
                speedChangeX = _movementSettings.Decceleration;
            }
            else
            {
                if (Mathf.Sign(relativeCurrentVelocity.x) != Mathf.Sign(relativeTargetVelocity.x))
                {
                    speedChangeX = _movementSettings.AccelerationForMovementInOppositeDirection;
                }
            }
            if (relativeTargetVelocity.z == 0.0f)
            {
                speedChangeZ = _movementSettings.Decceleration;
            }
            else
            {
                if (Mathf.Sign(relativeCurrentVelocity.z) != Mathf.Sign(relativeTargetVelocity.z))
                {
                    speedChangeZ = _movementSettings.AccelerationForMovementInOppositeDirection;
                }
            }

            relativeCurrentVelocity.x = Mathf.Lerp(relativeCurrentVelocity.x, relativeTargetVelocity.x, speedChangeX * deltaTime);
            relativeCurrentVelocity.y = Mathf.Lerp(relativeCurrentVelocity.y, relativeTargetVelocity.y, speedChangeY * deltaTime);
            relativeCurrentVelocity.z = Mathf.Lerp(relativeCurrentVelocity.z, relativeTargetVelocity.z, speedChangeZ * deltaTime);

            currentVelocity = transform.TransformDirection(relativeCurrentVelocity);

            rigidbody.velocity = currentVelocity;
        }

        public void SetPitchYawAndRoll(float pitch, float yaw, float roll)
        {
            _pitchRotation = pitch * _movementSettings.LookSensitivityX;
            _yawRotation = yaw * _movementSettings.LookSensitivityY;
            _rollRotation = roll * _movementSettings.BankingSensitivity;
        }

        public void SetMovementFactors(float moveHorizontal, float moveForward)
        {
            _movementDirection = new Vector3(moveHorizontal, 0.0f, moveForward);
        }

        private void StabilizeRollAxis(Transform transform, Rigidbody rigidbody, float deltaTime, out bool axisStabilized)
        {
            axisStabilized = false;

            Vector3 nearestWorldAxis = Math3DUtility.NearestWorldAxis(transform.up);

            _rotation = Quaternion.LookRotation(transform.forward, nearestWorldAxis);

            axisStabilized = Quaternion.Angle(rigidbody.rotation, _rotation) == 0.0f;
        }

        public void FreezeMovement()
        {
            _movementFreezeTimer = _movementSettings.DisableMovementAfterCollisionTime;
            _lastVelocity = Vector3.zero;
        }

        public void Bounce(Rigidbody rigidbody, Vector3 bounceNormal)
        {
            Vector3 reflected = Vector3.Reflect(_lastVelocity, bounceNormal) * _movementSettings.CollisionBounceForce;
            rigidbody.velocity = reflected;
        }
    }
}