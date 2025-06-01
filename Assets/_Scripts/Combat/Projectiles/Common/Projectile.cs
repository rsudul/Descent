using System;
using UnityEngine;

namespace Descent.Combat.Projectiles.Common
{
    public abstract class Projectile : MonoBehaviour
    {
        protected bool _isInitialized = false;
        protected bool _isSetToBeDestroyed = false;

        protected float _travelTimer = 0.0f;
        protected Vector3 _travelStartPosition = Vector3.zero;

        protected Collider _owner = null;

        [SerializeField]
        private ProjectileCollisionsController _collisionsController;
        [SerializeField]
        protected float _destroyAfterTimeInSeconds = 5.0f;
        [SerializeField]
        protected float _destroyAfterDistance = 1000.0f;

        public event EventHandler OnProjectileSetToBeDestroyed;

        protected void Initialize()
        {
            _travelStartPosition = transform.position;

            _isInitialized = true;
        }

        protected void Update()
        {
            if (_isInitialized && !_isSetToBeDestroyed)
            {
                CheckTravelTimer();
                CheckTravelDistance();
            }
        }

        protected void CheckTravelTimer()
        {
            _travelTimer += Time.deltaTime;

            if (_travelTimer >= _destroyAfterTimeInSeconds)
            {
                SetToBeDestroyed();
            }
        }

        protected void CheckTravelDistance()
        {
            if (Vector3.Distance(_travelStartPosition, transform.position) >= _destroyAfterDistance)
            {
                SetToBeDestroyed();
            }
        }

        protected void SetToBeDestroyed()
        {
            _isSetToBeDestroyed = true;
            OnProjectileSetToBeDestroyed?.Invoke(this, null);
            Destroy(gameObject, 0.1f);
        }

        public void SetOwner(Collider owner)
        {
            if (owner == null)
            {
                return;
            }

            _owner = owner;

            if (_collisionsController == null || _collisionsController.Collider == null)
            {
                return;
            }

            Physics.IgnoreCollision(_owner, _collisionsController.Collider);
        }
    }
}