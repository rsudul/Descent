using System.Collections.Generic;
using UnityEngine;
using Descent.Gameplay.Systems.WeaponSystem.Actions;
using Descent.Gameplay.Systems.WeaponSystem.Mods;
using Descent.Gameplay.Systems.WeaponSystem.Attachments;
using Descent.Gameplay.Systems.WeaponSystem.Model;

namespace Descent.Gameplay.Systems.WeaponSystem.Core
{
    public class Weapon : IWeapon
    {
        private List<WeaponAction> _actions = new List<WeaponAction>();
        private List<WeaponMod> _mods = new List<WeaponMod>();
        private Dictionary<AttachmentSlotData, List<Attachment>> _attachmentSlots =
            new Dictionary<AttachmentSlotData, List<Attachment>>();

        private int _selectedActionIndex = 0;

        public WeaponData WeaponData { get; private set; }
        public WeaponModel WeaponModel { get; private set; }
        public IWeaponOwner Owner { get; private set; }
        public float Damage { get; private set; }
        public float FireRate { get; private set; }
        public int MagazineSize { get; private set; }
        public float ReloadTime { get; private set; }
        public float Range { get; private set; }
        public int CurrentAmmo { get; private set; }

        public IReadOnlyList<WeaponAction> Actions => _actions;
        public IReadOnlyList<WeaponMod> Mods => _mods;
        public IReadOnlyDictionary<AttachmentSlotData, List<Attachment>> AttachmentSlots => _attachmentSlots;

        public WeaponAction SelectedAction => _actions.Count > 0 ? _actions[_selectedActionIndex] : null;

        public Weapon(WeaponData weaponData,
               IWeaponOwner owner,
               List<WeaponModData> startMods = null,
               Dictionary<AttachmentSlotData, List<AttachmentData>> startAttachments = null)
        {
            WeaponData = weaponData;
            Owner = owner;
            WeaponModel = GameObject.Instantiate(weaponData.WeaponModelPrefab,
                                                 Owner.WeaponMountPoint.position,
                                                 Owner.WeaponMountPoint.rotation,
                                                 Owner.WeaponMountPoint) as WeaponModel;
            if (WeaponModel == null)
            {
                Debug.Log($"[{WeaponData.WeaponName}][{Owner.WeaponMountPoint.root.name}]: WeaponModel could not be instantiated.");
                return;
            }
            Initialize(startMods, startAttachments);
        }

        private void Initialize(List<WeaponModData> startMods,
                                Dictionary<AttachmentSlotData, List<AttachmentData>> startAttachments)
        {
            if (WeaponData.WeaponActions != null)
            {
                foreach (WeaponActionData actionData in WeaponData.WeaponActions)
                {
                    _actions.Add(new WeaponAction(actionData));
                }
            }

            List<WeaponModData> modAssets = new List<WeaponModData>();
            if (WeaponData.BaseMods != null)
            {
                modAssets.AddRange(WeaponData.BaseMods);
            }
            if (startMods != null)
            {
                modAssets.AddRange(startMods);
            }
            foreach (WeaponModData modData in modAssets)
            {
                _mods.Add(new WeaponMod(modData));
            }

            if (WeaponData.AttachmentSlots != null)
            {
                foreach (AttachmentSlotData slot in WeaponData.AttachmentSlots)
                {
                    _attachmentSlots[slot] = new List<Attachment>();

                    if (startAttachments != null && startAttachments.TryGetValue(slot, out List<AttachmentData> attachments))
                    {
                        foreach (AttachmentData attachment in attachments)
                        {
                            TryAttachToSlot(attachment, slot);
                        }
                    }
                }
            }

            CalculateFinalStats();

            CurrentAmmo = MagazineSize;
        }

        protected void CalculateFinalStats()
        {
            Damage = WeaponData.BaseDamage;
            FireRate = WeaponData.BaseFireRate;
            Range = WeaponData.BaseRange;
            MagazineSize = WeaponData.BaseMagazineSize;
            ReloadTime = WeaponData.BaseReloadTime;

            foreach (AttachmentSlotData slot in _attachmentSlots.Keys)
            {
                foreach (Attachment attachment in _attachmentSlots[slot])
                {
                    attachment.Apply(this);
                }
            }

            foreach (WeaponMod mod in _mods)
            {
                mod.Apply(this);
            }
        }

        public void Fire()
        {
            if (SelectedAction == null)
            {
                return;
            }

            if (CurrentAmmo > 0)
            {
                SelectedAction.Execute(this);
                CurrentAmmo--;
            }
        }

        public void Reload()
        {
            CurrentAmmo = MagazineSize;
        }

        public bool TryAttachToSlot(AttachmentData data, AttachmentSlotData slot)
        {
            if (!_attachmentSlots.ContainsKey(slot))
            {
                return false;
            }

            List<Attachment> attachments = _attachmentSlots[slot];
            if (attachments.Count >= slot.MaxAttachments)
            {
                return false;
            }

            attachments.Add(new Attachment(data));
            CalculateFinalStats();
            return true;
        }

        public bool RemoveAttachmentFromSlot(AttachmentData data, AttachmentSlotData slot)
        {
            if (!_attachmentSlots.TryGetValue(slot, out List<Attachment> attachments))
            {
                return false;
            }

            Attachment toRemove = attachments.Find(a => a.Data == data);
            if (toRemove == null)
            {
                return false;
            }

            attachments.Remove(toRemove);
            CalculateFinalStats();

            return true;
        }

        public void AddMod(WeaponModData data)
        {
            _mods.Add(new WeaponMod(data));
            CalculateFinalStats();
        }
    }
}