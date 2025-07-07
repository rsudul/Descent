using Descent.Common.Events.Arguments;
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

            _healthController.OnDamaged += Damage;
            _healthController.OnRestoredHealth += Restore;
        }

        private void OnDestroy()
        {
            if (_healthController == null)
            {
                return;
            }

            _healthController.OnDamaged -= Damage;
            _healthController.OnRestoredHealth -= Restore;
        }

        private void Damage(object sender, DamageEventArgs args)
        {
            SetHealth();
        }

        private void Restore(object sender, RestoreHealthEventArguments args)
        {
            SetHealth();
        }

        private void SetHealth()
        {
            _healthbarFillImage.fillAmount = _healthController.Health / _healthController.MaxHealth;
        }
    }
}