using Descent.Combat.Projectiles.Common;
using Descent.Common.Collisions.Parameters;
using UnityEngine;

namespace Descent.Combat.Projectiles.BasicProjectile.Collisions
{
    public class BasicProjectileCollisionsController : ProjectileCollisionsController
    {
        protected override void OnCollided()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public override CollisionParameters GetCollisionParameters()
        {
            return new ProjectileCollisionParameters(25);
        }
    }
}