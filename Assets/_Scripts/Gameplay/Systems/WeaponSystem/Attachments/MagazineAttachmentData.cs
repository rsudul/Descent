using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Attachments
{
    [CreateAssetMenu(fileName = "MagazineAttachmentData", menuName = "Descent/Weapons/Attachments/Magazine")]
    public class MagazineAttachmentData : AttachmentData
    {
        [field: SerializeField] public int BonusMagazineSize { get; protected set; } = 0;

        public override void ApplyToWeapon(IWeapon weapon)
        {
            Debug.Log($"{AttachmentName} added to {weapon.WeaponData.WeaponName}");
        }
    }
}