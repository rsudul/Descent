namespace Descent.Gameplay.Systems.Alert
{
    public interface IAlertController
    {
        AlertLevel CurrentAlertLevel { get; }
        float AlertTimer { get; }
        float CombatTimer { get; }
        float SearchTimeRemaining { get; }

        void SetAlertLevel(AlertLevel alertLevel);
        void UpdateTimers(float deltaTime);
        void ResetTimers();
        void SetSearchDuration(float duration);
        void SetCooldownTimer(float duration);

        bool IsInState(AlertLevel alertLevel);
        bool TimerExceedsThreshold(float threshold);
    }
}