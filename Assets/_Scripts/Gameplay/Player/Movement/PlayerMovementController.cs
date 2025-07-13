using Descent.Extensions.Math;
using Descent.Gameplay.Movement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Descent.Gameplay.Player.Movement
{
    [System.Serializable]
    public class PlayerMovementController : IPlayerMovementController
    {
        private float _pitchRotation = 0.0f;
        private float _yawRotation = 0.0f;
        private float _rollRotation = 0.0f;
        private Quaternion _rotation = Quaternion.identity;
        private float _rollStabilizationTimer = 0.0f;
        private float _rollStabilizationDelay = 0.5f;
        private bool _isStabilizingAxis = false;

        private Vector3 _movementDirection = Vector3.zero;
        private float _movementDirectionActiveThreshold = 0.01f;
        private Vector3 _lastVelocity = Vector3.zero;

        private bool _isMoving = false;

        private float _movementFreezeTimer = 0.0f;

        private float _verticalInputThreshold = 0.01f;

        private float _postCollisionDriftTimer = 0.0f;
        private const float PostCollisionDriftDuration = 0.6f;

        private const float MovementThreshold = 0.001f;

        private Vector3 _smoothedMovementDirection = Vector3.zero;
        private Vector2 _smoothedLookInput = Vector2.zero;
        private float _smoothedBankingInput = 0.0f;

        public Vector3 Velocity => _lastVelocity;
        public bool IsMoving => _isMoving;

        private PlayerMovementSettings _movementSettings;

        public void Initialize(Transform transform, Rigidbody rigidbody, PlayerMovementSettings movementSettings)
        {
            _rotation = transform.rotation;
            _movementSettings = movementSettings;
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
            float lookDeadzone = _movementSettings.LookInputDeadzone;
            bool isLookingNow = Mathf.Abs(_pitchRotation) > lookDeadzone
                || Mathf.Abs(_yawRotation) > lookDeadzone || Mathf.Abs(_rollRotation) > lookDeadzone;

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

        public void UpdateMovement(Transform transform, Rigidbody rigidbody, float deltaTime,
            bool isTouchingWall, Vector3 wallNormal)
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
            _isMoving = currentVelocity.magnitude > MovementThreshold;

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
            else if (Mathf.Sign(relativeCurrentVelocity.x) != Mathf.Sign(relativeTargetVelocity.x))
            {
                speedChangeX = _movementSettings.AccelerationForMovementInOppositeDirection;
            }
            if (relativeTargetVelocity.y == 0.0f)
            {
                speedChangeY = _movementSettings.Decceleration;
            } else if (Mathf.Sign(relativeCurrentVelocity.y) != Mathf.Sign(-relativeTargetVelocity.y))
            {
                speedChangeY = _movementSettings.AccelerationForMovementInOppositeDirection;
            }
            if (relativeTargetVelocity.z == 0.0f)
            {
                speedChangeZ = _movementSettings.Decceleration;
            }
            else if (Mathf.Sign(relativeCurrentVelocity.z) != Mathf.Sign(relativeTargetVelocity.z))
            {
                speedChangeZ = _movementSettings.AccelerationForMovementInOppositeDirection;
            }

            float inertia = Mathf.Clamp01(_movementSettings.Inertia);

            relativeCurrentVelocity.x = Mathf.Lerp(relativeCurrentVelocity.x, relativeTargetVelocity.x,
                                                    (1.0f - inertia) * speedChangeX * deltaTime);
            relativeCurrentVelocity.y = Mathf.Lerp(relativeCurrentVelocity.y, relativeTargetVelocity.y,
                                                    (1.0f - inertia) * speedChangeY * deltaTime);
            relativeCurrentVelocity.z = Mathf.Lerp(relativeCurrentVelocity.z, relativeTargetVelocity.z,
                                                    (1.0f - inertia) * speedChangeZ * deltaTime);

            currentVelocity = transform.TransformDirection(relativeCurrentVelocity);

            rigidbody.velocity = currentVelocity;

            if (isTouchingWall)
            {
                rigidbody.velocity = ApplySlidingFriction(rigidbody.velocity, wallNormal);
            } else if (!isTouchingWall && !IsPlayerActivelySteering())
            {
                float speed = rigidbody.velocity.magnitude;

                float spaceDrag = _movementSettings.SpaceDragCurve != null
                    ? _movementSettings.SpaceDragCurve.Evaluate(speed)
                    : _movementSettings.SpaceDragFallback;

                rigidbody.velocity *= spaceDrag;

                if (rigidbody.velocity.magnitude < _movementSettings.SpaceDragSnapThreshold)
                {
                    rigidbody.velocity = Vector3.zero;
                }
            }
        }

        public void SetPitchYawAndRoll(float pitch, float yaw, float roll)
        {
            Vector2 targetLook = new Vector2(pitch, yaw);
            float lookResponsiveness = _movementSettings.LookResponsiveness;
            _smoothedLookInput = Vector2.Lerp(_smoothedLookInput, targetLook, Time.deltaTime * lookResponsiveness);

            float targetBanking = roll;
            float bankingResponsiveness = _movementSettings.BankingResponsiveness;
            _smoothedBankingInput = Mathf.Lerp(_smoothedBankingInput, targetBanking, Time.deltaTime * bankingResponsiveness);

            _pitchRotation = _smoothedLookInput.x * _movementSettings.LookSensitivityX;
            _yawRotation = _smoothedLookInput.y * _movementSettings.LookSensitivityY;
            _rollRotation = _smoothedBankingInput * _movementSettings.BankingSensitivity;
        }

        public void SetMovementFactors(float moveHorizontal, float moveForward, float moveVertical)
        {
            Vector3 targetDirection = new Vector3(moveHorizontal, moveVertical,
                Mathf.Abs(moveVertical) > _verticalInputThreshold ? 0.0f : moveForward);

            float responsiveness = _movementSettings.MovementResponsiveness;
            _smoothedMovementDirection = Vector3.Lerp(_smoothedMovementDirection, targetDirection, Time.deltaTime * responsiveness);

            _movementDirection = _smoothedMovementDirection;
        }

        private void StabilizeRollAxis(Transform transform, Rigidbody rigidbody, float deltaTime, out bool axisStabilized)
        {
            axisStabilized = false;

            Vector3 nearestWorldAxis = Math3DUtility.NearestWorldAxis(transform.up);

            _rotation = Quaternion.LookRotation(transform.forward, nearestWorldAxis);

            axisStabilized = Quaternion.Angle(rigidbody.rotation, _rotation) == 0.0f;
        }

        private void FreezeMovement()
        {
            _movementFreezeTimer = _movementSettings.DisableMovementAfterCollisionTime;
            _lastVelocity = Vector3.zero;
        }

        public void Bounce(Rigidbody rigidbody, Vector3 bounceNormal)
        {
            Vector3 velocity = _lastVelocity;

            float normalComponent = Vector3.Dot(velocity.normalized, bounceNormal);

            if (Mathf.Abs(normalComponent) > _movementSettings.NormalBounceThreshold)
            {
                Vector3 vN = Vector3.Project(velocity, bounceNormal);
                Vector3 vT = (velocity - vN) * _movementSettings.TangentialFriction;

                Vector3 bouncedNormal = -vN * _movementSettings.NormalBounceDamping;

                Vector3 finalVelocity = vT + bouncedNormal;

                finalVelocity *= _movementSettings.CollisionBounceForce;

                rigidbody.velocity = finalVelocity;
            }
            else
            {
                rigidbody.velocity = ApplySlidingFriction(velocity, bounceNormal);
            }
        }

        public void OnCollisionImpact(Rigidbody rigidbody, float impactSpeed, Vector3 impactNormal)
        {
            if (impactSpeed < _movementSettings.ImpactSpeedReactionThreshold)
            {
                return;
            }

            Bounce(rigidbody, impactNormal);

            if (impactSpeed > _movementSettings.ImpactSpeedStunThreshold)
            {
                FreezeMovement();
            }
        }

        private Vector3 ApplySlidingFriction(Vector3 velocity, Vector3 wallNormal)
        {
            Vector3 vN = Vector3.Project(velocity, wallNormal);
            Vector3 vT = velocity - vN;

            float slidingFriction = _movementSettings.SlidingFrictionCurve != null
                ? _movementSettings.SlidingFrictionCurve.Evaluate(vT.magnitude)
                : _movementSettings.TangentialFriction;

            Vector3 vTnew = vT * slidingFriction;

            if (vTnew.magnitude < _movementSettings.SlidingFrictionSnapThreshold)
            {
                vTnew = Vector3.zero;
            }

            return vN + vTnew;
        }

        private bool IsPlayerActivelySteering()
        {
            return _movementDirection.sqrMagnitude > _movementDirectionActiveThreshold;
        }

        private void PostCollisionDrift(float deltaTime)
        {
            if (_postCollisionDriftTimer > 0.0f)
            {
                _postCollisionDriftTimer -= deltaTime;
                _isStabilizingAxis = false;

                float driftProgress = _postCollisionDriftTimer / PostCollisionDriftDuration;
                float wobbleStrength = Mathf.Lerp(0.8f, 0.0f, 1 - driftProgress);

                float yawWobble = Mathf.Sin(Time.time * 13.7f) * wobbleStrength;
                float rollWobble = Mathf.Sin(Time.time * 8.1f) * wobbleStrength;

                _rotation *= Quaternion.AngleAxis(yawWobble, Vector3.up);
                _rotation *= Quaternion.AngleAxis(rollWobble, Vector3.forward);
            }
        }
    }
}