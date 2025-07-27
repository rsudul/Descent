using UnityEngine;

namespace Descent.Gameplay.Systems.WeaponSystem.Core
{
    public interface IWeaponOwner
    {
        GameObject GameObject { get; }
        Transform WeaponMountPoint { get; }
    }
}