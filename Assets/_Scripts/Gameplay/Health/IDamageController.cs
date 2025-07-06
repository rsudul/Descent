using Descent.Common.Events.Arguments;
using System;
using UnityEngine;

namespace Descent.Gameplay.Health
{
    public interface IDamageController
    {
        event EventHandler<DamageEventArgs> OnDamaged;
        void TakeDamage(float amount, Vector3 sourcePosition);
    }
}