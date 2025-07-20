using UnityEngine;

namespace Descent.Gameplay.Systems.Perception
{
    [CreateAssetMenu(fileName = "PerceptionSettings", menuName = "Descent/Perception/Settings")]
    public class PerceptionSettings : ScriptableObject
    {
        [field: SerializeField] public float Range { get; private set; } = 20.0f;
        [field: SerializeField] public float ViewAngle { get; private set; } = 120.0f;
        [field: SerializeField] public LayerMask DetectionMask { get; private set; }

        public PerceptionSettings Clone()
        {
            PerceptionSettings clone = CreateInstance<PerceptionSettings>();
            clone.Range = Range;
            clone.ViewAngle = ViewAngle;
            clone.DetectionMask = DetectionMask;
            return clone;
        }
    }
}