using Descent.Common.Collisions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Gameplay.DamageableObjects
{
    public class DamageableObject : MonoBehaviour, IDamageable
    {
        public event EventHandler OnDamageTaken;
        public event EventHandler OnDied;

        [SerializeField]
        private List<HitController> _hitControllers = new List<HitController>();
        [SerializeField]
        private int _lifePoints = 0;

        private void Awake()
        {
            foreach (HitController hitController in _hitControllers)
            {
                hitController.OnHit += TakeDamage;
            }
        }

        private void TakeDamage(object sender, EventArgs e)
        {
            TakeDamage(0);
            OnDamageTaken?.Invoke(this, null);
        }

        public void TakeDamage(int damage)
        {
            _lifePoints -= damage;
            if (_lifePoints < 0)
            {
                _lifePoints = 0;
            }

            if (_lifePoints == 0)
            {
                OnDied?.Invoke(this, null);
            }
        }
    }
}