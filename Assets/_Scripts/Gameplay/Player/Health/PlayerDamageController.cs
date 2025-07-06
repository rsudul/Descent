using System;
using UnityEngine;
using Descent.Common.Events.Arguments;
using Descent.Gameplay.Health;

namespace Descent.Gameplay.Player.Health
{
    public class PlayerDamageController : MonoBehaviour, IDamageController
    {
        public event EventHandler<DamageEventArgs> OnDamaged;

        public void TakeDamage(float amount, Vector3 sourcePosition)
        {
            OnDamaged?.Invoke(this, new DamageEventArgs(amount, sourcePosition));
        }
    }
}