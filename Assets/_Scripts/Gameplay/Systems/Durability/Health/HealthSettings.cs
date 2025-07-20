using UnityEngine;

namespace Descent.Gameplay.Systems.Durability.Health
{
    [CreateAssetMenu(fileName = "HealthSettings", menuName = "Descent/Health/Settings")]
    public class HealthSettings : ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; } = 100.0f;
        [field: SerializeField] public float MaxHealth { get; private set; } = 100.0f;

        public HealthSettings Clone()
        {
            HealthSettings clone = CreateInstance<HealthSettings>();
            clone.Health = Health;
            clone.MaxHealth = MaxHealth;
            return clone;
        }
    }
}