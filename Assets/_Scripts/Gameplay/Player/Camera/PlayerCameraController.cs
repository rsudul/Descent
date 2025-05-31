using UnityEngine;

namespace Descent.Gameplay.Player.Camera
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _playerRigidbody;

        private void LateUpdate()
        {
            if (_playerRigidbody == null)
            {
                return;
            }

            transform.position = _playerRigidbody.transform.position;
            transform.rotation = _playerRigidbody.transform.rotation;
        }
    }
}