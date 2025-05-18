using System;
using UnityEngine;
using Descent.Common.Collisions.Parameters;

namespace Descent.Common.Collisions.Controllers
{
    [RequireComponent(typeof(Collider))]
    public class HitController : MonoBehaviour
    {
        public event EventHandler<CollisionParameters> OnHit;

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.transform.TryGetComponent<ICollisionParametersProvider>(out ICollisionParametersProvider collisionParametersProvider))
            {
                return;
            }

            CollisionParameters collisionParameters = collisionParametersProvider.GetCollisionParameters();
            if (collisionParameters == null)
            {
                return;
            }

            OnHit?.Invoke(this, collisionParameters);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent<ICollisionParametersProvider>(out ICollisionParametersProvider collisionParametersProvider))
            {
                return;
            }

            CollisionParameters collisionParameters = collisionParametersProvider.GetCollisionParameters();
            if (collisionParameters == null)
            {
                return;
            }

            OnHit?.Invoke(this, collisionParameters);
        }
    }
}