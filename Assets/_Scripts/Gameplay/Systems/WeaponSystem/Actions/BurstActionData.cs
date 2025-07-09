using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;
using Descent.Gameplay.Systems.WeaponSystem.Projectiles;

namespace Descent.Gameplay.Systems.WeaponSystem.Actions
{
    [CreateAssetMenu(fileName = "BurstActionData", menuName = "Descent/Weapons/Actions/Burst")]
    public class BurstActionData : WeaponActionData
    {
        [field: SerializeField] public ProjectileData ProjectileData { get; protected set; }
        [field: SerializeField] public float Cooldown { get; protected set; } = 0.0f;
        [field: SerializeField] public int BurstCount { get; protected set; } = 0;
        [field: SerializeField] public float BurstInterval { get; protected set; } = 0.1f;

        public override void Execute(IWeapon weapon)
        {
            Debug.Log($"{weapon.WeaponData.WeaponName} fires burst of {BurstCount} shots");
        }
    }
}