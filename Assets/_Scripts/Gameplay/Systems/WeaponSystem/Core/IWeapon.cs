using System.Collections.Generic;
using Descent.Gameplay.Systems.WeaponSystem.Actions;
using Descent.Gameplay.Systems.WeaponSystem.Mods;
using Descent.Gameplay.Systems.WeaponSystem.Attachments;
using Descent.Gameplay.Systems.WeaponSystem.Model;

namespace Descent.Gameplay.Systems.WeaponSystem.Core
{
    public interface IWeapon
    {
        WeaponData WeaponData { get; }
        WeaponModel WeaponModel { get; }
        IWeaponOwner Owner { get; }
        float Damage { get; }
        float FireRate { get; }
        int MagazineSize { get; }
        float ReloadTime { get; }
        float Range { get; }
        int CurrentAmmo { get; }

        IReadOnlyList<WeaponAction> Actions { get; }
        WeaponAction SelectedAction { get; }

        IReadOnlyList<WeaponMod> Mods { get; }
        IReadOnlyDictionary<AttachmentSlotData, List<Attachment>> AttachmentSlots { get; }

        void Fire();
        void Reload();

        bool TryAttachToSlot(AttachmentData data, AttachmentSlotData slot);
        bool RemoveAttachmentFromSlot(AttachmentData data, AttachmentSlotData slot);

        void AddMod(WeaponModData data);
    }
}