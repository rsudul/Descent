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
        [SerializeField, Tooltip("If set to false, the object will pass through other objects.")]
        private bool _actAsSolidBody = false;

        public void Initialize(Vector3 movementDirection)
        {
            InitializeControllers(movementDirection);
        }

        private void InitializeControllers(Vector3 movementDirection)
        {
            _movementController = new BasicProjectileMovementController();
            _movementController.Initialize(_movementSettings);
            _movementController.PrepareMovement(movementDirection);

            if (_collisionController != null)
            {
                _collisionController.Initialize(actAsSolidBody: _actAsSolidBody);
            }
        }

        public void StartMovement(float deltaTime)
        {
            _movementController.StartMovement(_rigidbody, deltaTime);
        }
    }
}