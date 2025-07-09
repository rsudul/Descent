using System;
using UnityEngine;

namespace Descent.Gameplay.Systems.WeaponSystem.Core
{
    public abstract class WeaponSystemController : IWeaponSystemController
    {
        public IWeapon CurrentWeapon { get; protected set; }

        public event EventHandler<IWeapon> OnWeaponEquipped;
        public event EventHandler OnFired;
        public event EventHandler OnReloaded;

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