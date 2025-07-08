using Descent.Gameplay.Collisions;
using Descent.Gameplay.Events.Arguments;
using System;
using UnityEngine;

namespace Descent.Gameplay.Player.Collisions
{
    [Serializable]
    public class PlayerCollisionsController : ICollisionParametersProvider
    {
        private Rigidbody _rigidbody = null;
        private HitController _hitController = null;

        public Collider Collider => _collider;

        [SerializeField]
        private Collider _collider = null;

        public event EventHandler<CollisionEventArgs> OnCollision;

        ~PlayerCollisionsController()
        {
            if (_hitController != null)
            {
                _hitController.OnHit -= OnHit;
            }
        }

        public void Initialize(HitController hitController, Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;

            _hitController = hitController;
            _hitController.OnHit += OnHit;
        }

        private void OnHit(object sender, CollisionEventArgs args)
        {
            OnCollision?.Invoke(this, args);
        }

        public CollisionParameters GetCollisionParameters()
        {
            return null;
        }
    }
}