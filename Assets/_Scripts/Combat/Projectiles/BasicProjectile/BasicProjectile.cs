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

        private void InitializeControllers()
        {
            _movementController = new BasicProjectileMovementController();
            _movementController.Initialize(_movementSettings);
        }
    }
}