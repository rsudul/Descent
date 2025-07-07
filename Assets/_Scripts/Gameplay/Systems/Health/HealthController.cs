using Descent.Common.Events.Arguments;
using Descent.Gameplay.Systems.Health.Settings;
using System;
using UnityEngine;

namespace Descent.Gameplay.Systems.Health
{
    public class HealthController : IHealthController
    {
        private HealthSettings _settings;

        public float Health { get; private set; }
        public float MaxHealth {  get; private set; }
        public bool IsAlive {  get; private set; }

        public event EventHandler<DamageEventArgs> OnDamaged;
        public event EventHandler<RestoreHealthEventArguments> OnRestoredHealth;
        public event EventHandler OnDied;

        public HealthController(HealthSettings settings)
        {
            _settings = settings.Clone();
            Health = _settings.Health;
            MaxHealth = _settings.MaxHealth;
            IsAlive = true;
        }

        public void Heal(float amount)
        {
            if (!IsAlive)
            {
                return;
            }

            Health = MathF.Min(Health + amount, MaxHealth);
            RestoreHealthEventArguments args = new RestoreHealthEventArguments();
            OnRestoredHealth?.Invoke(this, args);
        }

        public void RestoreToFullHealth()
        {
            if (!IsAlive)
            {
                return;
            }

            Health = MaxHealth;
            RestoreHealthEventArguments args = new RestoreHealthEventArguments();
            OnRestoredHealth?.Invoke(this, args);
        }

        public void TakeDamage(float amount, Vector3 sourcePosition)
        {
            if (!IsAlive)
            {
                return;
            }
            Health = Mathf.Max(Health - amount, 0.0f);

            OnDamaged?.Invoke(this, new DamageEventArgs(amount, sourcePosition));

            if (Health == 0.0f)
            {
                OnDied?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}