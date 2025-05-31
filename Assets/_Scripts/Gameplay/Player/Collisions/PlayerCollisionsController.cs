using Descent.Common.Collisions.Controllers;
using Descent.Common.Collisions.Parameters;
using Descent.Common.Events.Arguments;
using System;
using UnityEngine;

namespace Descent.Gameplay.Player.Collisions
{
    [Serializable]
    public class PlayerCollisionsController : ICollisionParametersProvider
    {
        private Rigidbody _rigidbody = null;
        private HitController _hitController = null;

        public event EventHandler<CollisionEventArguments> OnCollision;

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

        private void OnHit(object sender, CollisionEventArguments args)
        {
            OnCollision?.Invoke(this, args);
        }

        public CollisionParameters GetCollisionParameters()
        {
            return null;
        }
    }
}