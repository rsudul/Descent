using Descent.Common.Events.Arguments;
using System;
using UnityEngine;

namespace Descent.Gameplay.Systems.Health
{
    public interface IHealthController
    {
        float Health { get; }
        float MaxHealth { get; }

        bool IsAlive { get; }

        event EventHandler<DamageEventArgs> OnDamaged;
        event EventHandler<RestoreHealthEventArguments> OnRestoredHealth;
        event EventHandler OnDied;

        void Heal(float amount);
        void RestoreToFullHealth();
        void TakeDamage(float amount, Vector3 sourcePosition);
    }
}