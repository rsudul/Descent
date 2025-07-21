namespace Descent.Gameplay.Movement
{
    public interface IAIRotationController : IRotationController
    {
        void RotateTo(float targetYAngle);
        void StopRotation();
    }
}