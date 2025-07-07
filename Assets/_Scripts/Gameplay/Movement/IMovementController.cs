using UnityEngine;

namespace Descent.Gameplay.Movement
{
    public interface IMovementController
    {
        Vector3 Velocity { get; }
        bool IsMoving { get; }
    }
}