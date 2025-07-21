using UnityEngine;

namespace Descent.Gameplay.Enemies.Turret
{
    [CreateAssetMenu(fileName = "DefaultTurretSettings", menuName = "Descent/Enemies/Turret Settings")]
    public class TurretSettings : ScriptableObject
    {
        [field: SerializeField] public float ScanCenterAngle { get; private set; } = 0.0f;
        [field: SerializeField] public float ScanAngle { get; private set; } = 0.0f;
        [field: SerializeField] public float WaitTimeOnEdge { get; private set; } = 0.0f;
    }
}