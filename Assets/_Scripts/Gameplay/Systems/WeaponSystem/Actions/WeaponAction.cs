using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Actions
{
    public class WeaponAction
    {
        public WeaponActionData Data { get; }

        public WeaponAction(WeaponActionData data)
        {
            Data = data;
        }

        public void Execute(IWeapon weapon)
        {
            Data.Execute(weapon);
        }
    }
}