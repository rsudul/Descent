using ProjectSC.Combat.Projectiles.BasicProjectile;
using ProjectSC.Common.ObjectSpawning;

using UnityEngine;

namespace ProjectSC.Gameplay.Player.Combat
{
    public class PlayerShootingController
    {
        public void Shoot(Vector3 forwardVector, Vector3 spawnPosition, float deltaTime)
        {
            // method hides ObjectSpawner
            BasicProjectile projectile = ObjectSpawner.SpawnObjectAtPosition(ObjectSpawnType.BasicProjectile, spawnPosition).
                                            GetComponent<BasicProjectile>();

            if (projectile != null)
            {
                projectile.Initialize(movementDirection: forwardVector);
                projectile.StartMovement(deltaTime);
            }
        }
    }
}