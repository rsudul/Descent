using System;
using UnityEngine;
using Descent.Common.Collisions.Parameters;
using Descent.Common.Events.Arguments;

namespace Descent.Common.Collisions.Controllers
{
    [RequireComponent(typeof(Collider))]
    public class HitController : MonoBehaviour
    {
        public event EventHandler<CollisionEventArguments> OnHit;

        private void OnCollisionEnter(Collision collision)
        {
            CollisionParameters collisionParameters = null;

            if (collision.transform.TryGetComponent<ICollisionParametersProvider>(out ICollisionParametersProvider collisionParametersProvider))
            {
                collisionParameters = collisionParametersProvider.GetCollisionParameters();
            }

            CollisionEventArguments eventArgs = new CollisionEventArguments(collision.transform, collision.contacts[0].point,
                collision.contacts[0].normal, collisionParameters, false);

            OnHit?.Invoke(this, eventArgs);
        }

        private void OnTriggerEnter(Collider other)
        {
            CollisionParameters collisionParameters = null;

            if (other.transform.TryGetComponent<ICollisionParametersProvider>(out ICollisionParametersProvider collisionParametersProvider))
            {

                collisionParameters = collisionParametersProvider.GetCollisionParameters();
            }

            CollisionEventArguments eventArgs = new CollisionEventArguments(other.transform, Vector3.zero, Vector3.zero,
                collisionParameters, true);

            OnHit?.Invoke(this, eventArgs);
        }
    }
}