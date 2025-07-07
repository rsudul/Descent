using UnityEngine;

namespace Descent.Gameplay.Movement
{
    public interface IAIMovementController : IMovementController
    {
        void MoveTo(Vector3 targetPosition);
        void Stop();
    }
}