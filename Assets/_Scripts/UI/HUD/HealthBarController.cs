using Descent.Gameplay.Events.Arguments;
using Descent.Gameplay.Systems.Health;
using UnityEngine;
using UnityEngine.UI;

namespace Descent.UI.HUD
{
    public class HealthBarController : MonoBehaviour
    {
        private IHealthController _healthController = null;

        [SerializeField]
        private Image _healthbarFillImage;

        public void Initialize(IHealthController healthController)
        {
            _healthController = healthController;

            if (_healthController == null)
            {
                return;
            }
        }

        private void OnDestroy()
        {
            if (_healthController == null)
            {
                return;
            }
        }

        private void Damage(object sender, DamageEventArgs args)
        {
            SetHealth();
        }

        private void Restore(object sender, HealthChangedEventArgs args)
        {
            SetHealth();
        }

        private void SetHealth()
        {
            _healthbarFillImage.fillAmount = _healthController.Health / _healthController.MaxHealth;
        }
    }
}