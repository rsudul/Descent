using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Mods
{
    public abstract class WeaponModData : ScriptableObject
    {
        [field: SerializeField] public string ModName { get; protected set; } = string.Empty;
        [field: SerializeField, TextArea] public string Description { get; protected set; } = string.Empty;

        public abstract void Apply(IWeapon weapon);
    }
}