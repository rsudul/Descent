using UnityEngine;

namespace Descent.Combat.Projectiles.BasicProjectile.Settings.Movement
{
    [CreateAssetMenu(fileName = "BasicProjectileMovementSettings", menuName = "Descent/Combat/Projectiles/BasicProjectile/Settings/Movement")]
    public class BasicProjectileMovementSettings : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; } = 500.0f;
    }
}