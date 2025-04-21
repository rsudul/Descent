using Descent.Combat.Projectiles.BasicProjectile;
using Descent.Common.ObjectSpawning;

using UnityEngine;

namespace Descent.Gameplay.Player.Combat
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
                projectile.Initialize(movementDirection: forwardVector, orientation: forwardVector);
                projectile.StartMovement(deltaTime);
            }
        }
    }
}