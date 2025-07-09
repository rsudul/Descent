using UnityEngine;

namespace Descent.Gameplay.Systems.WeaponSystem.Projectiles
{
    [CreateAssetMenu(fileName = "Projectile", menuName = "Descent/Weapons/Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        [field: SerializeField] public string ProjectileName { get; private set; } = string.Empty;
        [field: SerializeField] public GameObject Prefab { get; private set; } = null;
        [field: SerializeField] public float BaseDamage { get; private set; } = 0.0f;
        [field: SerializeField] public float Speed { get; private set; } = 0.0f;
    }
}