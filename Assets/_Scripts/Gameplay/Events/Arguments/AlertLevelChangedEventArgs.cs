using Descent.Gameplay.Systems.Alert;
using System;

namespace Descent.Gameplay.Events.Arguments
{
    public class AlertLevelChangedEventArgs : EventArgs
    {
        public AlertLevel PreviousLevel { get; }
        public AlertLevel NewLevel { get; }
        public float TimeSinceLastChange { get; }

        public AlertLevelChangedEventArgs(AlertLevel previousLevel, AlertLevel newLevel, float timeSinceLastChange)
        {
            PreviousLevel = previousLevel;
            NewLevel = newLevel;
            TimeSinceLastChange = timeSinceLastChange;
        }
    }
}