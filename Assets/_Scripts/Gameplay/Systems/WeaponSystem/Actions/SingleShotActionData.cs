using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;
using Descent.Gameplay.Systems.WeaponSystem.Projectiles;
using Descent.Gameplay.Combat.Projectiles.BasicProjectile;

namespace Descent.Gameplay.Systems.WeaponSystem.Actions
{
    [CreateAssetMenu(fileName = "SingleShotActionData", menuName = "Descent/Weapons/Actions/SingleShot")]
    public class SingleShotActionData : WeaponActionData
    {
        [field: SerializeField] public ProjectileData ProjectileData { get; protected set; }
        [field: SerializeField] public float Cooldown { get; protected set; } = 0.0f;

        public override void Execute(IWeapon weapon)
        {
            if (ProjectileData == null)
            {
                Debug.Log($"[{ActionName}] ProjectileData is null.");
                return;
            }

            if (weapon == null)
            {
                Debug.LogError($"[{ActionName}] Weapon is null.");
                return;
            }

            if (weapon.WeaponModel == null)
            {
                Debug.Log("weaponmodel is null");
                return;
            }

            if (weapon.WeaponModel.FirePoint == null)
            {
                Debug.Log("firepoint is null");
                return;
            }

            Vector3 projectileSpawnPoint = weapon.WeaponModel.FirePoint.position;
            Vector3 projectileSpawnOrientation = weapon.WeaponModel.FirePoint.forward;
            BasicProjectile projectile = Instantiate(ProjectileData.Prefab, projectileSpawnPoint,
                                            Quaternion.Euler(projectileSpawnOrientation)).GetComponent<BasicProjectile>();
            projectile.SetOwner(weapon.Owner.GameObject);
            projectile.Initialize(projectileSpawnOrientation, projectileSpawnOrientation);
            projectile.StartMovement(Time.deltaTime);
        }
    }
}