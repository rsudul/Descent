using Descent.Gameplay.Systems.WeaponSystem.Core;

namespace Descent.Gameplay.Systems.WeaponSystem.Attachments
{
    public class Attachment
    {
        public AttachmentData Data { get; }

        public Attachment(AttachmentData data)
        {
            Data = data;
        }

        public void Apply(IWeapon weapon)
        {
            Data.ApplyToWeapon(weapon);
        }
    }
}