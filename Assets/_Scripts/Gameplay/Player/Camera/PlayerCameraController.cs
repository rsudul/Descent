using Descent.Gameplay.Effects.Camera;
using UnityEngine;

namespace Descent.Gameplay.Player.Camera
{
    public class PlayerCameraController : MonoBehaviour
    {
        public Transform CameraTransform => _cameraTransform;

        [SerializeField]
        private Rigidbody _playerRigidbody;
        [SerializeField]
        private CameraShake _cameraShake;
        [SerializeField]
        private Transform _cameraTransform;

        private void LateUpdate()
        {
            if (_playerRigidbody == null)
            {
                return;
            }

            transform.position = _playerRigidbody.transform.position;
            transform.rotation = _playerRigidbody.transform.rotation;
        }

        public void Shake(float shakeStrength)
        {
            if (_cameraShake == null)
            {
                return;
            }

            _cameraShake.Shake(shakeStrength);
        }
    }
}