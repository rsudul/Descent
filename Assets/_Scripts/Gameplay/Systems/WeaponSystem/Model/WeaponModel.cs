using UnityEngine;

namespace Descent.Gameplay.Systems.WeaponSystem.Model
{
    public class WeaponModel : MonoBehaviour
    {
        public Transform FirePoint => _firePoint;

        [SerializeField]
        private Transform _firePoint;
    }
}