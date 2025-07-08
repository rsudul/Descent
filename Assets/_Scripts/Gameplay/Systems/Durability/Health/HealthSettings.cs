using UnityEngine;

namespace Descent.Gameplay.Systems.Durability.Health
{
    [CreateAssetMenu(fileName = "HealthSettings", menuName = "Descent/Health/Settings")]
    public class HealthSettings : ScriptableObject
    {
        public float Health = 100.0f;
        public float MaxHealth = 100.0f;

        public HealthSettings Clone()
        {
            HealthSettings clone = new HealthSettings();
            clone.Health = Health;
            clone.MaxHealth = MaxHealth;
            return clone;
        }
    }
}