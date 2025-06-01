using Descent.Combat.Projectiles.Common;
using Descent.Common.Collisions.Parameters;
using System;
using UnityEngine;

namespace Descent.Combat.Projectiles.BasicProjectile.Collisions
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