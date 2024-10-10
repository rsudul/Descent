using ProjectSC.Gameplay.Player.Settings.Movement;
using UnityEngine;

namespace ProjectSC.Gameplay.Player.Movement
{
    [System.Serializable]
    public class PlayerMovementController
    {
        private float _pitchRotation = 0.0f;
        private float _yawRotation = 0.0f;
        private Quaternion _localRotation = Quaternion.identity;

        private Vector3 _movementDirection = Vector3.zero;

        [Header("Settings")]
        [SerializeField]
        private PlayerMovementSettings _movementSettings;

        public void Initialize(Transform transform, Rigidbody rigidbody)
        {
            _localRotation = transform.localRotation;
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

        public void UpdateLook(Transform transform, float deltaTime)
        {
            _localRotation *= Quaternion.Euler(-_yawRotation, _pitchRotation, 0.0f);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, _localRotation, _movementSettings.LookSmoothness * deltaTime);
        }

        public void UpdateMovement(Transform transform, Rigidbody rigidbody, float deltaTime)
        {
            Vector3 currentVelocity = rigidbody.velocity;

            Vector3 targetVelocity = rigidbody.rotation * _movementDirection;

            targetVelocity = targetVelocity * _movementSettings.MovementSpeed;

            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, _movementSettings.Acceleration * deltaTime);

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
    }
}