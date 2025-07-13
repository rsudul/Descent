using System;
using UnityEngine;
using Descent.Gameplay.Events.Arguments;

namespace Descent.Gameplay.Collisions
{
    [RequireComponent(typeof(Collider))]
    public class HitController : MonoBehaviour
    {
        public event EventHandler<CollisionEventArgs> OnHit;
        public event EventHandler<CollisionEventArgs> OnHitStay;
        public event EventHandler<CollisionEventArgs> OnHitExit;

        private void OnCollisionEnter(Collision collision)
        {
            CollisionParameters collisionParameters = null;

            if (collision.transform.TryGetComponent<ICollisionParametersProvider>(out ICollisionParametersProvider collisionParametersProvider))
            {
                collisionParameters = collisionParametersProvider.GetCollisionParameters();
            }

            CollisionEventArgs eventArgs = new CollisionEventArgs(collision.transform, collision.contacts[0].point,
                collision.contacts[0].normal, collision.relativeVelocity, collisionParameters, false);

            OnHit?.Invoke(this, eventArgs);
        }

        private void OnTriggerEnter(Collider other)
        {
            CollisionParameters collisionParameters = null;

            if (other.transform.TryGetComponent<ICollisionParametersProvider>(out ICollisionParametersProvider collisionParametersProvider))
            {

                collisionParameters = collisionParametersProvider.GetCollisionParameters();
            }

            CollisionEventArgs eventArgs = new CollisionEventArgs(other.transform, Vector3.zero, Vector3.zero, Vector3.zero,
                collisionParameters, true);

            OnHit?.Invoke(this, eventArgs);
        }

        private void OnCollisionStay(Collision collision)
        {
            CollisionParameters collisionParameters = null;

            if (collision.transform.TryGetComponent(out ICollisionParametersProvider collisionParametersProvider))
            {
                collisionParameters = collisionParametersProvider.GetCollisionParameters();
            }

            CollisionEventArgs eventArgs = new CollisionEventArgs(
                collision.transform,
                collision.contacts[0].point,
                collision.contacts[0].normal,
                collision.relativeVelocity,
                collisionParameters,
                false);

            OnHitStay?.Invoke(this, eventArgs);
        }

        private void OnCollisionExit(Collision collision)
        {
            CollisionParameters collisionParameters = null;

            if (collision.transform.TryGetComponent(out ICollisionParametersProvider collisionParametersProvider))
            {
                collisionParameters = collisionParametersProvider.GetCollisionParameters();
            }

            CollisionEventArgs eventArgs = new CollisionEventArgs(
                collision.transform,
                Vector3.zero,
                Vector3.zero,
                Vector3.zero,
                collisionParameters,
                false);

            OnHitExit?.Invoke(this, eventArgs);
        }
    }
}