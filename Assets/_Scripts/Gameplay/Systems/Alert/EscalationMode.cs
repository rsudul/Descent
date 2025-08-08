namespace Descent.Gameplay.Systems.Alert
{
    public enum EscalationMode
    {
        Automatic,          // use AIAlertController.EscalateAlert() logic
        ToSpecificLevel,    // jump to specific alert level
        OneStep             // move up exactly one level
    }
}