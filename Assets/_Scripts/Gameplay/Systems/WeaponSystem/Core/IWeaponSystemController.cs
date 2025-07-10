using System;

namespace Descent.Gameplay.Systems.WeaponSystem.Core
{
    public interface IWeaponSystemController
    {
        IWeapon CurrentWeapon { get; }

        event EventHandler<IWeapon> OnWeaponEquipped;
        event EventHandler OnFired;
        event EventHandler OnReloaded;

        void Fire();
        void Reload();
        void EquipWeapon(IWeapon weapon);
    }
}