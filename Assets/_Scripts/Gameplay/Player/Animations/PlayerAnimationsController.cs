using Descent.Gameplay.Player.Settings.Animations;
using UnityEngine;

namespace Descent.Gameplay.Player.Animations
{
    [System.Serializable]
    public class PlayerAnimationsController
    {
        private Quaternion _originalLocalRotation = Quaternion.identity;
        private Vector3 _movementVelocity = Vector3.zero;

        private Transform _playerCameraPivot = null;

        [Header("Settings")]
        [SerializeField]
        private PlayerAnimationsSettings _animationsSettings;

        public void Initialize(Transform playerCameraPivot)
        {
            _playerCameraPivot = playerCameraPivot;

            _originalLocalRotation = _playerCameraPivot.localRotation;
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

            _playerCameraPivot.localRotation = Quaternion.Lerp(_playerCameraPivot.localRotation, targetLocalRotation,
                deltaTime * _animationsSettings.TiltSpeed);
        }
    }
}