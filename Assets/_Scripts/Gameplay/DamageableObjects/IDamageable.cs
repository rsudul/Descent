using System;

namespace Descent.Gameplay.DamageableObjects
{
    public interface IDamageable
    {
        event EventHandler OnDamageTaken;
        event EventHandler OnDied;

        void TakeDamage(int damage);
    }
}