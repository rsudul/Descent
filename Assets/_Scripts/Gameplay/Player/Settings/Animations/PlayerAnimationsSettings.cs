using UnityEngine;

namespace Descent.Gameplay.Player.Settings.Animations
{
    [CreateAssetMenu(fileName = "PlayerAnimationsSettings", menuName = "Descent/Player/Settings/Animations")]
    public class PlayerAnimationsSettings : ScriptableObject
    {
        [field: SerializeField] public float MaxSidewaysTiltAngle { get; private set; } = 15.0f;
        [field: SerializeField] public float MaxForwardTiltAngle { get; private set; } = 10.0f;
        [field: SerializeField] public float TiltSpeed { get; private set; } = 2.0f;
    }
}