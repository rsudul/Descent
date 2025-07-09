using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Mods
{
    [CreateAssetMenu(fileName = "FireModData", menuName = "Descent/Weapons/Mods/Fire")]
    public class FireModData : WeaponModData
    {
        [field: SerializeField] public float FireDamageBonuds { get; protected set; } = 0.0f;

        public override void Apply(IWeapon weapon)
        {
            Debug.Log($"{ModName}: Fire bonus {FireDamageBonuds} applied to {weapon.WeaponData.WeaponName}");
        }
    }
}