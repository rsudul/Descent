using System;
using System.Collections.Generic;

namespace Descent.Gameplay.Systems.WeaponSystem.Core
{
    public class WeaponSystemController : IWeaponSystemController
    {
        private List<Weapon> _weapons;

        public IWeapon CurrentWeapon { get; protected set; }
        public IReadOnlyList<Weapon> Weapons => _weapons;

        public event EventHandler<IWeapon> OnWeaponEquipped;
        public event EventHandler OnFired;
        public event EventHandler OnReloaded;

        public void Initialize(WeaponsConfig weaponsConfig)
        {
            _weapons = new List<Weapon>();
            foreach (WeaponData weaponData in weaponsConfig.StartingWeapons)
            {
                Weapon weapon = new Weapon(weaponData, null);
                _weapons.Add(weapon);
            }
        }

        public virtual void Fire()
        {
            if (CurrentWeapon == null)
            {
                return;
            }

            CurrentWeapon.Fire();
            OnFired?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Reload()
        {
            if (CurrentWeapon == null)
            {
                return;
            }

            CurrentWeapon.Reload();
            OnReloaded?.Invoke(this, EventArgs.Empty);
        }

        public virtual void EquipWeapon(IWeapon weapon)
        {
            CurrentWeapon = weapon;
            OnWeaponEquipped?.Invoke(this, CurrentWeapon);
        }
    }
}