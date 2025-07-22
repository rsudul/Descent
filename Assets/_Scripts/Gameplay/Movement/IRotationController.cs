namespace Descent.Gameplay.Movement
{
    public interface IRotationController
    {
        float RotationSpeed { get; }
        bool IsRotating { get; }
        float CurrentYAngle { get; }
    }
}