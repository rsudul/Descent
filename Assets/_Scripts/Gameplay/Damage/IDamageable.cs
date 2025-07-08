using Descent.Gameplay.Events.Arguments;
using System;
using UnityEngine;

namespace Descent.Gameplay.DamageableObjects
{
    public interface IDamageable
    {
        event EventHandler<DamageEventArgs> OnDamageTaken;
        event EventHandler OnDied;

        GameObject GameObject { get; }

        void TakeDamage(int damage);
    }
}