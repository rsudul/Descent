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

        public bool IsTouchingWall { get; private set; } = false;
        public Vector3 LastCollisionNormal { get; private set; } = Vector3.zero;

        public Collider Collider => _collider;

        [SerializeField]
        private Collider _collider = null;

        public event EventHandler<CollisionEventArgs> OnCollision;

        ~PlayerCollisionsController()
        {
            if (_hitController != null)
            {
                _hitController.OnHit -= OnHit;
                _hitController.OnHitStay -= OnHitStay;
                _hitController.OnHitExit -= OnHitExit;
            }
        }

        public void Initialize(HitController hitController, Rigidbody rigidbody)
        {
            _rigidbody = rigidbody;

            _hitController = hitController;
            _hitController.OnHit += OnHit;
            _hitController.OnHitStay += OnHitStay;
            _hitController.OnHitExit += OnHitExit;
        }

        private void OnHit(object sender, CollisionEventArgs args)
        {
            if (args.IsTrigger)
            {
                return;
            }

            IsTouchingWall = true;
            LastCollisionNormal = args.CollisionNormal;

            OnCollision?.Invoke(this, args);
        }

        private void OnHitStay(object sender, CollisionEventArgs args)
        {
            if (args.IsTrigger)
            {
                return;
            }

            IsTouchingWall = true;
            LastCollisionNormal = args.CollisionNormal;
        }

        private void OnHitExit(object sender, CollisionEventArgs args)
        {
            if (args.IsTrigger)
            {
                return;
            }

            IsTouchingWall = false;
            LastCollisionNormal = Vector3.zero;
        }

        public CollisionParameters GetCollisionParameters()
        {
            return null;
        }
    }
}