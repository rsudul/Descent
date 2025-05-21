using Descent.Combat.Projectiles.BasicProjectile.Collisions;
using Descent.Combat.Projectiles.BasicProjectile.Movement;
using Descent.Combat.Projectiles.BasicProjectile.Settings.Movement;
using Descent.Combat.Projectiles.Common;
using UnityEngine;

namespace Descent.Combat.Projectiles.BasicProjectile
{
    public class BasicProjectile : Projectile
    {
        private BasicProjectileMovementController _movementController = null;

        [SerializeField]
        private Rigidbody _rigidbody = null;

        [SerializeField]
        private BasicProjectileMovementSettings _movementSettings = null;

        [Header("Collisions")]
        [SerializeField]
        private BasicProjectileCollisionsController _collisionController = null;

        public void Initialize(Vector3 movementDirection, Vector3 orientation)
        {
            InitializeControllers(movementDirection, orientation);
            transform.rotation = Quaternion.LookRotation(orientation);

            base.Initialize();
        }

        private void InitializeControllers(Vector3 movementDirection, Vector3 orientation)
        {
            _movementController = new BasicProjectileMovementController();
            _movementController.Initialize(_movementSettings);
            _movementController.PrepareMovement(movementDirection);
            _movementController.OrientateInDirection(transform, orientation);

            if (_collisionController != null)
            {
                _collisionController.Initialize();
            }
        }

        public void StartMovement(float deltaTime)
        {
            _movementController.StartMovement(_rigidbody, deltaTime);
        }
    }
}