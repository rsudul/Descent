using UnityEngine;

namespace Descent.Gameplay.Player.Animations
{
    [System.Serializable]
    public class PlayerAnimationsController
    {
        private Quaternion _originalLocalRotation = Quaternion.identity;
        private Vector3 _movementVelocity = Vector3.zero;

        private Transform _playerCamera = null;

        [Header("Settings")]
        [SerializeField]
        private PlayerAnimationsSettings _animationsSettings;

        public void Initialize(Transform playerCamera)
        {
            _playerCamera = playerCamera;

            _originalLocalRotation = _playerCamera.localRotation;
        }

        public void SetMovementVelocity(Vector3 movementVelocity)
        {
            _movementVelocity = movementVelocity;
        }

        public void UpdateAnimations(float deltaTime)
        {
            float targetSidewaysTilt = -_movementVelocity.x * _animationsSettings.MaxSidewaysTiltAngle;
            float targetForwardTilt = _movementVelocity.z * _animationsSettings.MaxForwardTiltAngle;

            Quaternion targetLocalRotation = _originalLocalRotation * Quaternion.Euler(targetForwardTilt, 0.0f, targetSidewaysTilt);

            _playerCamera.localRotation = Quaternion.Lerp(_playerCamera.localRotation, targetLocalRotation,
                deltaTime * _animationsSettings.TiltSpeed);
        }
    }
}