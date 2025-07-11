using UnityEngine;

namespace Descent.Gameplay.Movement
{
    public interface IPlayerMovementController : IMovementController
    {
        void UpdateLook(Transform transform, Rigidbody rigidbody, float deltaTime);
        void UpdateMovement(Transform transform, Rigidbody rigidbody, float deltaTime);
        void OnCollisionImpact(Rigidbody rigidbody, float impactSpeed, Vector3 impactNormal);
    }
}