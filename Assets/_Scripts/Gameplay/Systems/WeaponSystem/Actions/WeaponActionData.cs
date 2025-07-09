using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Actions
{
    public abstract class WeaponActionData : ScriptableObject
    {
        [field: SerializeField] public string ActionName { get; protected set; } = string.Empty;
        [field: SerializeField, TextArea] public string Description { get; protected set; } = string.Empty;

        public abstract void Execute(IWeapon weapon);
    }
}