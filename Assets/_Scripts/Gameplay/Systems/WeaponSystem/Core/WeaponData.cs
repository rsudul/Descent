using System.Collections.Generic;
using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Actions;
using Descent.Gameplay.Systems.WeaponSystem.Mods;
using Descent.Gameplay.Systems.WeaponSystem.Model;
using Descent.Gameplay.Systems.WeaponSystem.Attachments;

namespace Descent.Gameplay.Systems.WeaponSystem.Core
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Descent/Weapons/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField, Header("Basic info")] public string WeaponName { get; private set; } = string.Empty;
        [field: SerializeField, TextArea] public string Description { get; private set; } = string.Empty;

        [field: SerializeField, Header("Stats")] public float BaseDamage { get; private set; } = 0.0f;
        [field: SerializeField] public float BaseFireRate { get; private set; } = 0.0f;
        [field: SerializeField] public float BaseRange { get; private set; } = 0.0f;
        [field: SerializeField] public int BaseMagazineSize { get; private set; } = 0;
        [field: SerializeField] public float BaseReloadTime { get; private set; } = 0.0f;

        [field: SerializeField, Header("Weapon actions")] public List<WeaponActionData> WeaponActions { get; private set; }

        [field:SerializeField, Header("Base mods")] public List<WeaponModData> BaseMods { get; private set; }

        [field: SerializeField, Header("Attachment slots")] public List<AttachmentSlotData> AttachmentSlots { get; private set; }

        [field: SerializeField, Header("Prefab")] public WeaponModel WeaponModelPrefab { get; private set; }
    }
}