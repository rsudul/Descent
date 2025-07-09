using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;
using Descent.Gameplay.Systems.WeaponSystem.Projectiles;

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
                Debug.Log($"{weapon.WeaponData.WeaponName} fires single shot.");
            }
        }
    }
}