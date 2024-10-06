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

        /*
         * TODO:
         * Move this to separate class, i.e. PlayerMovementSettings
        */
        [Header("Look")]
        [SerializeField]
        private float _lookSensitivityX = 3.5f;
        [SerializeField]
        private float _lookSensitivityY = 3.5f;
        [SerializeField]
        private float _lookSmoothness = 64.0f;
        [SerializeField]
        private float _movementSpeed = 4.0f;
        [SerializeField]
        private float _acceleration = 1.0f;

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

            transform.localRotation = Quaternion.Slerp(transform.localRotation, _localRotation, _lookSmoothness * deltaTime);
        }

        public void UpdateMovement(Transform transform, Rigidbody rigidbody, float deltaTime)
        {
            Vector3 currentVelocity = rigidbody.velocity;

            Vector3 targetVelocity = rigidbody.rotation * _movementDirection;

            targetVelocity = targetVelocity * _movementSpeed;

            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, _acceleration * deltaTime);

            rigidbody.velocity = currentVelocity;
        }

        public void SetPitchAndYaw(float pitch, float yaw)
        {
            _pitchRotation = pitch * _lookSensitivityX;
            _yawRotation = yaw * _lookSensitivityY;
        }

        public void SetMovementFactors(float moveHorizontal, float moveForward)
        {
            _movementDirection = new Vector3(moveHorizontal, 0.0f, moveForward);
        }
    }
}