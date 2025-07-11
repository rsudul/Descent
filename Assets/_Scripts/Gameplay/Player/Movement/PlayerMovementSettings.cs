using UnityEngine;

namespace Descent.Gameplay.Player.Movement
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Descent/Player/Settings/Movement")]
    public class PlayerMovementSettings : ScriptableObject
    {
        [field: SerializeField, Header("Look")] public float LookSensitivityX { get; private set; } = 3.5f;
        [field: SerializeField] public float LookSensitivityY { get; private set; } =  3.5f;
        [field: SerializeField] public float BankingSensitivity { get; private set; } = 3.5f;
        [field: SerializeField] public float LookSmoothness { get; private set; } = 64.0f;
        [field: SerializeField] public float RollAxisResetSpeed { get; private set; } = 2.0f;

        [field: SerializeField, Header("Movement")] public float MovementSpeed { get; private set; } = 4.0f;
        [field: SerializeField] public float Acceleration { get; private set; } = 12.0f;
        [field: SerializeField] public float AccelerationForMovementInOppositeDirection { get; private set; } = 9.0f;
        [field: SerializeField] public float Decceleration { get; private set; } = 4.5f;

        [field: SerializeField, Header("Collisions")] public float DisableMovementAfterCollisionTime { get; private set; } = 0.5f;
        [field: SerializeField] public float CollisionBounceForce { get; private set; } = 100.0f;
        [field: SerializeField] public float ImpactSpeedReactionThreshold { get; private set; } = 2.5f;
        [field: SerializeField] public float ImpactSpeedStunThreshold { get; private set; } = 9.0f;
        [field: SerializeField] public float TangentialFriction { get; private set; } = 0.98f;
        [field: SerializeField] public float NormalBounceDamping { get; private set; } = 0.6f;
        [field: SerializeField] public float NormalBounceThreshold { get; private set; } = 0.3f;
    }
}