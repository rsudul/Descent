using Descent.Gameplay.Collisions;
using Descent.Gameplay.Events.Arguments;
using System;
using UnityEngine;

namespace Descent.Gameplay.Combat.Projectiles.Base
{
    [RequireComponent(typeof(Collider))]
    public abstract class ProjectileCollisionsController : MonoBehaviour, ICollisionParametersProvider
    {
        public Collider Collider => _collider;

        [SerializeField]
        private Collider _collider = null;
        [SerializeField, Tooltip("If set to false, the object will pass through other objects.")]
        private bool _actAsSolidBody = false;
        [SerializeField]
        private HitController _hitController = null;

        public event EventHandler OnCollisionEntered;

        public virtual void Initialize()
        {
            if (_collider == null)
            {
                // pass error message
                return;
            }

            _collider.isTrigger = !_actAsSolidBody;

            if (_hitController != null)
            {
                _hitController.OnHit += OnCollided;
            }
        }

        protected virtual void OnCollided(object sender, CollisionEventArgs args)
        {
            OnCollisionEntered?.Invoke(this, EventArgs.Empty);
        }

        public virtual CollisionParameters GetCollisionParameters()
        {
            return null;
        }
    }
}