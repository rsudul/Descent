using Descent.Common.Events.Arguments;
using System;

namespace Descent.Gameplay.Systems.Health
{
    public interface IHealthController
    {
        float Health { get; }
        float MaxHealth { get; }

        bool IsAlive { get; }

        event EventHandler<HealthChangedEventArgs> OnHealthChanged;
        event EventHandler OnDied;

        void Heal(float amount);
        void RestoreToFullHealth();
    }
}