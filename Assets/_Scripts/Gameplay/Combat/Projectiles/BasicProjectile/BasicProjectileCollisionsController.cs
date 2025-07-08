using Descent.Common.Collisions.Parameters;
using Descent.Gameplay.Combat.Projectiles.Base;
using System;

namespace Descent.Gameplay.Combat.Projectiles.BasicProjectile
{
    public class BasicProjectileCollisionsController : ProjectileCollisionsController
    {
        protected override void OnCollided(object sender, EventArgs args)
        {
            base.OnCollided(sender, args);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public override CollisionParameters GetCollisionParameters()
        {
            return new ProjectileCollisionParameters(25);
        }
    }
}