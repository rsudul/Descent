using System;
using UnityEngine;

namespace Descent.Gameplay.DamageableObjects
{
    public interface IDamageable
    {
        event EventHandler OnDamageTaken;
        event EventHandler OnDied;

        GameObject GameObject { get; }

        void TakeDamage(int damage);
    }
}