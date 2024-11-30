using UnityEngine;

namespace ProjectSC.Combat.Projectiles.Common
{
    [RequireComponent(typeof(Collider))]
    public abstract class ProjectileCollisionsController : MonoBehaviour
    {
        // move collider to another script?
        private Collider _collider;

        public void Initialize(bool actAsSolidBody)
        {
            _collider = GetComponent<Collider>();

            if (_collider == null)
            {
                // pass error message
                return;
            }

            _collider.isTrigger = actAsSolidBody;
        }

        private void OnCollisionEnter(Collision collision)
        {

        }
    }
}