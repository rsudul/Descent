using Descent.Extensions.Math;
using Descent.Gameplay.Movement;
using UnityEngine;

namespace Descent.Gameplay.Player.Movement
{
    [System.Serializable]
    public class PlayerMovementController : IPlayerMovementController
    {
        private PlayerMovementSettings _movementSettings;

        private float _pitchRotation = 0.0f;
        private float _yawRotation = 0.0f;
        private float _rollRotation = 0.0f;

        private Vector3 _movementDirection = Vector3.zero;
        private float _movementDirectionActiveThreshold = 0.01f;
        private Vector3 _lastVelocity = Vector3.zero;

        private bool _isMoving = false;

        private float _movementFreezeTimer = 0.0f;

        private float _verticalInputThreshold = 0.01f;

        private const float MovementThreshold = 0.001f;

        private Vector3 _smoothedMovementDirection = Vector3.zero;
        private Vector2 _smoothedLookInput = Vector2.zero;
        private float _smoothedBankingInput = 0.0f;

        private int _currentSpeedLevel = 0;
        private int _speedLevelsCount = 6;
        private float[] _speedLevelMultipliers = null;
        private int _defaultSpeedLevel = 2;
        private int _minimalSpeedLevel = 2;

        public Vector3 Velocity => _lastVelocity;
        public bool IsMoving => _isMoving;

        public int CurrentSpeedLevel => _currentSpeedLevel;
        public int MaxSpeedLevel => _speedLevelsCount;

        public void Initialize(Transform transform, Rigidbody rigidbody, PlayerMovementSettings movementSettings)
        {
            _movementSettings = movementSettings;
            InitializeRigidbody(rigidbody);

            _speedLevelsCount = _movementSettings.SpeedLevelsCount;
            _speedLevelMultipliers = _movementSettings.SpeedLevelMultipliers;
            _defaultSpeedLevel = Mathf.Clamp(_movementSettings.DefaultSpeedLevel, 0, _speedLevelsCount - 1);
            _currentSpeedLevel = _defaultSpeedLevel;
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
            Quaternion inputRotation = rigidbody.rotation;
            inputRotation *= Quaternion.AngleAxis(-_yawRotation, Vector3.right);
            inputRotation *= Quaternion.AngleAxis(_pitchRotation, Vector3.up);
            inputRotation *= Quaternion.AngleAxis(_rollRotation, Vector3.forward);

            Quaternion targetRotation = inputRotation;

            bool isRollInputActive = Mathf.Abs(_smoothedBankingInput) > _movementSettings.LookInputDeadzone;

            if (!isRollInputActive)
            {
                targetRotation = StabilizeRollAxis(inputRotation);
            }

            float rotationSpeed = _movementSettings.LookSmoothness;

            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, deltaTime * rotationSpeed);
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
            float speedMultiplier = 1.0f;
            if (_speedLevelMultipliers != null && _speedLevelMultipliers.Length > _currentSpeedLevel)
            {
                speedMultiplier = _speedLevelMultipliers[_currentSpeedLevel];
            }

            targetVelocity = targetVelocity * (_movementSettings.MovementSpeed * speedMultiplier);

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
            float snapThreshold = _movementSettings.VelocitySnapThreshold;

            if (Mathf.Abs(relativeCurrentVelocity.x - relativeTargetVelocity.x) > snapThreshold)
            {
                relativeCurrentVelocity.x = Mathf.Lerp(relativeCurrentVelocity.x, relativeTargetVelocity.x,
                    (1.0f - inertia) * speedChangeX * deltaTime);
            }
            else
            {
                relativeCurrentVelocity.x = Mathf.MoveTowards(relativeCurrentVelocity.x, relativeTargetVelocity.x,
                    speedChangeX * deltaTime);
            }
            if (Mathf.Abs(relativeCurrentVelocity.y - relativeTargetVelocity.y) > snapThreshold)
            {
                relativeCurrentVelocity.y = Mathf.Lerp(relativeCurrentVelocity.y, relativeTargetVelocity.y,
                    (1.0f - inertia) * speedChangeY * deltaTime);
            }
            else
            {
                relativeCurrentVelocity.y = Mathf.MoveTowards(relativeCurrentVelocity.y, relativeTargetVelocity.y,
                    speedChangeY * deltaTime);
            }
            if (Mathf.Abs(relativeCurrentVelocity.z - relativeTargetVelocity.z) > snapThreshold)
            {
                relativeCurrentVelocity.z = Mathf.Lerp(relativeCurrentVelocity.z, relativeTargetVelocity.z,
                    (1.0f - inertia) * speedChangeZ * deltaTime);
            }
            else
            {
                relativeCurrentVelocity.z = Mathf.MoveTowards(relativeCurrentVelocity.z, relativeTargetVelocity.z,
                    speedChangeZ * deltaTime);
            }

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

        private Quaternion StabilizeRollAxis(Quaternion currentRotation)
        {
            Vector3 nearestWorldAxis = Math3DUtility.NearestWorldAxis(currentRotation * Vector3.up);
            return Quaternion.LookRotation(currentRotation * Vector3.forward, nearestWorldAxis);
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

        public void IncreaseSpeedLevel()
        {
            if (_currentSpeedLevel < _speedLevelsCount - 1)
            {
                _currentSpeedLevel++;
            }
        }

        public void DecreaseSpeedLevel()
        {
            if (_currentSpeedLevel > _minimalSpeedLevel)
            {
                _currentSpeedLevel--;
            }
        }

        public void ResetSpeedLevel()
        {
            _currentSpeedLevel = _defaultSpeedLevel;
        }
    }
}