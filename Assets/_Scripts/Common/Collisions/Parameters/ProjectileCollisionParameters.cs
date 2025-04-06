using Descent.Gameplay.DamageableObjects;

namespace Descent.Common.Collisions.Parameters
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