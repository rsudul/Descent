using Descent.Gameplay.Systems.Durability.Damage;
using Descent.Gameplay.Collisions;

namespace Descent.Gameplay.Combat.Projectiles.Base
{
    public class ProjectileCollisionParameters : CollisionParameters, IDamageDealer
    {
        public int Damage { get; protected set; }
        public DamageType DamageType => DamageType.Projectile;

        public ProjectileCollisionParameters(int damage)
        {
            Damage = damage;
        }
    }
}