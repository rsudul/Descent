using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Attachments
{
    public abstract class AttachmentData : ScriptableObject
    {
        [field: SerializeField] public string AttachmentName { get; protected set; } = string.Empty;
        [field: SerializeField, TextArea] public string Description { get; protected set; } = string.Empty;

        public abstract void ApplyToWeapon(IWeapon weapon);
    }
}