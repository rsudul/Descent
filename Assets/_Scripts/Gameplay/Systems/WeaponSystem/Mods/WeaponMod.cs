using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Mods
{
    public class WeaponMod
    {
        public WeaponModData Data { get; }

        public WeaponMod(WeaponModData data)
        {
            Data = data;
        }

        public void Apply(IWeapon weapon)
        {
            Data.Apply(weapon);
        }
    }
}