using ProjectSC.Gameplay.Player.Settings.Movement;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectSC.Gameplay.Player.Movement
{
    [System.Serializable]
    public class PlayerMovementController
    {
        private float _pitchRotation = 0.0f;
        private float _yawRotation = 0.0f;
        private Quaternion _rotation = Quaternion.identity;
        private bool _beginLookingAround = false;
        private bool _lastBeginLookingAround = false;
        private bool _doneLookingAround = false;

        private Vector3 _movementDirection = Vector3.zero;

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
            rigidbody.freezeRotation = false;
            rigidbody.useGravity = false;
            rigidbody.drag = 0.0f;
            rigidbody.angularDrag = 0.05f;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        public void UpdateLook(Transform transform, Rigidbody rigidbody, float deltaTime)
        {
            if (!_beginLookingAround)
            {
                _beginLookingAround = _yawRotation != 0.0f || _pitchRotation != 0.0f;
            }

            if (_beginLookingAround)
            {
                if (_doneLookingAround)
                {
                    _doneLookingAround = false;
                }

                if (!_lastBeginLookingAround)
                {
                    _rotation = rigidbody.rotation;
                }

                _rotation *= Quaternion.AngleAxis(-_yawRotation, Vector3.right);
                _rotation *= Quaternion.AngleAxis(_pitchRotation, Vector3.up);

                rigidbody.MoveRotation(Quaternion.Lerp(rigidbody.rotation, _rotation, _movementSettings.LookSmoothness * deltaTime));

                float remainingAngle = Quaternion.Angle(rigidbody.rotation, _rotation);
                if (remainingAngle == 0.0f)
                {
                    Debug.Log("done looking around");
                    _beginLookingAround = false;
                    _doneLookingAround = true;
                }
            }

            if (_doneLookingAround)
            {
                StabilizeRollAxis(transform, rigidbody, deltaTime, out bool axisStabilized);
                _doneLookingAround = !axisStabilized;
            }

            _lastBeginLookingAround = _beginLookingAround;
        }

        public void UpdateMovement(Transform transform, Rigidbody rigidbody, float deltaTime)
        {
            Vector3 currentVelocity = rigidbody.velocity;

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

        public void SetPitchAndYaw(float pitch, float yaw)
        {
            _pitchRotation = pitch * _movementSettings.LookSensitivityX;
            _yawRotation = yaw * _movementSettings.LookSensitivityY;
        }

        public void SetMovementFactors(float moveHorizontal, float moveForward)
        {
            _movementDirection = new Vector3(moveHorizontal, 0.0f, moveForward);
        }

        private void StabilizeRollAxis(Transform transform, Rigidbody rigidbody, float deltaTime, out bool axisStabilized)
        {
            axisStabilized = false;

            Vector3 nearestWorldAxis = NearestWorldAxis(transform.up);

            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, nearestWorldAxis);
            rigidbody.MoveRotation(Quaternion.Slerp(rigidbody.rotation, targetRotation, _movementSettings.RollAxisResetSpeed * deltaTime));

            axisStabilized = Quaternion.Angle(rigidbody.rotation, targetRotation) == 0.0f;
        }

        private Vector3 NearestWorldAxis(Vector3 vector)
        {
            List<Vector3> worldVectors = new List<Vector3>() { Vector3.up, Vector3.down, Vector3.left,
                    Vector3.right, Vector3.back, Vector3.forward };
            float dotProduct = -Mathf.Infinity;
            Vector3 worldVectorToAlignWith = Vector3.zero;

            foreach (Vector3 worldVector in worldVectors)
            {
                float tempDotProduct = Vector3.Dot(vector, worldVector);
                if (tempDotProduct > dotProduct)
                {
                    dotProduct = tempDotProduct;
                    worldVectorToAlignWith = worldVector;
                }
            }

            return worldVectorToAlignWith;
        }
    }
}