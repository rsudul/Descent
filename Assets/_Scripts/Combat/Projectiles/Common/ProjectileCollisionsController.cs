using Descent.Common.Collisions.Parameters;
using System;
using UnityEngine;

namespace Descent.Combat.Projectiles.Common
{
    [RequireComponent(typeof(Collider))]
    public abstract class ProjectileCollisionsController : MonoBehaviour, ICollisionParametersProvider
    {
        public Collider Collider => _collider;

        [SerializeField]
        private Collider _collider = null;
        [SerializeField, Tooltip("If set to false, the object will pass through other objects.")]
        private bool _actAsSolidBody = false;

        public event EventHandler OnCollisionEntered;

        public virtual void Initialize()
        {
            if (_collider == null)
            {
                // pass error message
                return;
            }

            _collider.isTrigger = !_actAsSolidBody;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            OnCollided();
            OnCollisionEntered?.Invoke(this, null);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            OnCollided();
            OnCollisionEntered?.Invoke(this, null);
        }

        protected virtual void OnCollided()
        {

        }

        public virtual CollisionParameters GetCollisionParameters()
        {
            return null;
        }
    }
}