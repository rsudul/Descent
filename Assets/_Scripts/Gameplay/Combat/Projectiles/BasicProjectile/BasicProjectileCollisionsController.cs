using Descent.Gameplay.Collisions;
using Descent.Gameplay.Combat.Projectiles.Base;
using Descent.Gameplay.Events.Arguments;

namespace Descent.Gameplay.Combat.Projectiles.BasicProjectile
{
    public class BasicProjectileCollisionsController : ProjectileCollisionsController
    {
        protected override void OnCollided(object sender, CollisionEventArgs args)
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