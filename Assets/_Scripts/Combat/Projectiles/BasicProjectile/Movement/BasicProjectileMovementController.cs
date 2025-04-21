using Descent.Combat.Projectiles.BasicProjectile.Settings.Movement;
using UnityEngine;

namespace Descent.Combat.Projectiles.BasicProjectile.Movement
{
    public class BasicProjectileMovementController
    {
        private Vector3 _movementDirection = Vector3.zero;

        private BasicProjectileMovementSettings _settings = null;

        public void Initialize(BasicProjectileMovementSettings settings)
        {
            _settings = settings;
        }

        private void InitializeRigidbody(Rigidbody rigidbody)
        {
            rigidbody.drag = 0.0f;
            rigidbody.angularDrag = 0.05f;
            rigidbody.useGravity = false;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        public void PrepareMovement(Vector3 movementDirection)
        {
            _movementDirection = movementDirection;
        }

        public void OrientateInDirection(Transform transform, Vector3 oritentation)
        {
            transform.rotation = Quaternion.LookRotation(oritentation);
        }

        public void StartMovement(Rigidbody rigidbody, float deltaTime)
        {
            rigidbody.velocity = _movementDirection * _settings.MovementSpeed * deltaTime;
        }
    }
}