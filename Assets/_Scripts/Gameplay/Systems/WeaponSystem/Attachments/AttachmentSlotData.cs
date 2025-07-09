using UnityEngine;

namespace Descent.Gameplay.Systems.WeaponSystem.Attachments
{
    [CreateAssetMenu(fileName = "AttachmentSlotData", menuName = "Descent/Weapons/Attachment Slot")]
    public class AttachmentSlotData : ScriptableObject
    {
        [field: SerializeField] public string SlotName { get; protected set; } = string.Empty;
        [field: SerializeField] public int MaxAttachments { get; protected set; } = 0;
    }
}