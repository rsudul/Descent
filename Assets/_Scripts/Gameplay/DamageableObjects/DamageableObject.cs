using Descent.Gameplay.Collisions;
using Descent.Common.Events.Arguments;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Gameplay.DamageableObjects
{
    public class DamageableObject : MonoBehaviour, IDamageable
    {
        public event EventHandler<DamageEventArgs> OnDamageTaken;
        public event EventHandler OnDied;

        public GameObject GameObject { get { return gameObject; } }

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

        private void TakeDamage(object sender, CollisionEventArguments args)
        {
            IDamageDealer damageDealer = args.CollisionParameters as IDamageDealer;
            if (damageDealer == null)
            {
                return;
            }

            DamageEventArgs damageArgs = new DamageEventArgs(damageDealer.Damage, args.CollisionPoint);

            TakeDamage(damageDealer.Damage);
            OnDamageTaken?.Invoke(this, damageArgs);
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