using UnityEngine;

namespace Descent.Gameplay.Movement
{
    public interface IMovementController
    {
        void MoveTo(Vector3 targetPosition);
        void Stop();
        Vector3 Velocity { get; }
        bool IsMoving { get; }
    }
}