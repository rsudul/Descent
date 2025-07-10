using Descent.Gameplay.Events.Arguments;
using Descent.Gameplay.Game;
using Descent.Gameplay.Player;
using Descent.Gameplay.Systems.Durability.Damage;
using UnityEngine;

namespace Descent.UI.HUD
{
    public class UIDamageIndicatorController : MonoBehaviour
    {
        private Player _player;
        private IDamageable _damageable;

        [SerializeField]
        private GameObject _indicatorPrefab;
        [SerializeField]
        private Transform _indicatorParent;

        private void Start()
        {
            if (!GameController.Instance.GetPlayer(out GameObject playerGameObject))
            {
                return;
            }

            if (!playerGameObject.TryGetComponent<Player>(out _player))
            {
                return;
            }

            if (!_player.TryGetComponent<IDamageable>(out _damageable))
            {
                return;
            }

            _damageable.OnDamageTaken += OnPlayerDamaged;
        }

        private void OnDestroy()
        {
            if (_damageable != null)
            {
                _damageable.OnDamageTaken -= OnPlayerDamaged;
            }
        }

        private void OnPlayerDamaged(object sender, DamageEventArgs args)
        {
            ShowIndicator(args.SourcePosition);
        }

        private void ShowIndicator(Vector3 sourcePosition)
        {
            Vector3 playerPosition = _player.transform.position;
            Vector3 directionToSource = (sourcePosition -  playerPosition).normalized;

            Vector3 forward = _player.transform.forward;
            Vector3 right = _player.transform.right;

            directionToSource.y = 0.0f;
            forward.y = 0.0f;
            right.y = 0.0f;
            directionToSource.Normalize();
            forward.Normalize();
            right.Normalize();

            float angle = Vector3.SignedAngle(forward, directionToSource, Vector3.up);

            float canvasRadius = 250.0f;

            float radians = angle * Mathf.Deg2Rad;
            Vector2 indicatorPos = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians)) * canvasRadius;

            GameObject indicator = Instantiate(_indicatorPrefab, _indicatorParent);
            RectTransform rt = indicator.GetComponent<RectTransform>();
            rt.anchoredPosition = indicatorPos;
            rt.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);

            Destroy(indicator, 1.2f);
        }
    }
}