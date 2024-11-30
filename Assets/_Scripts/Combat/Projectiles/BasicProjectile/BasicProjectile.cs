using ProjectSC.Combat.Projectiles.BasicProjectile.Collisions;
using ProjectSC.Combat.Projectiles.BasicProjectile.Movement;
using ProjectSC.Combat.Projectiles.BasicProjectile.Settings.Movement;
using ProjectSC.Combat.Projectiles.Common;
using UnityEngine;

namespace ProjectSC.Combat.Projectiles.BasicProjectile
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
        [SerializeField]
        private bool _actAsSolidBody = false;

        private void InitializeControllers()
        {
            _movementController = new BasicProjectileMovementController();
            _movementController.Initialize(_movementSettings);

            if (_collisionController != null)
            {
                _collisionController.Initialize(actAsSolidBody: _actAsSolidBody);
            }
        }
    }
}