namespace Descent.Gameplay.Systems.Alert
{
    public interface ISuspicionController
    {
        float SuspicionLevel { get; }

        void UpdateSuspicion(float increaseRate, float decayRate, bool hasTarget, float deltaTime);
        bool ExceedsThreshold(float threshold);
        void SetSuspicionLevel(float suspicionLevel);
        void ResetSuspicion();
    }
}