namespace Descent.Constants
{
    public static class PinNames
    {
        // Alert System Pins
        public const string CURRENT_ALERT_LEVEL = "CurrentAlertLevel";
        public const string TARGET_ALERT_LEVEL = "TargetAlertLevel";
        public const string SUSPICION_LEVEL = "SuspicionLevel";
        public const string SUSPICION_THRESHOLD = "SuspicionThreshold";

        // Timer Pins
        public const string SEARCH_DURATION = "SearchDuration";
        public const string COOLDOWN_DURATION = "CooldownDuration";
        public const string ALERT_TIMER = "AlertTimer";
        public const string COMBAT_TIMER = "CombatTimer";
        public const string SEARCH_TIME_REMAINING = "SearchTimeRemaining";

        // Position Pins
        public const string LAST_KNOWN_POSITION = "LastKnownPosition";
        public const string TARGET_POSITION = "TargetPosition";

        // Combat Pins
        public const string ANGLE_THRESHOLD = "AngleThreshold";
        public const string LEAD_TIME = "LeadTime";
        public const string HAS_ACTIVE_TARGET = "HasActiveTarget";

        // Configuration Pins
        public const string SCAN_ANGLE = "ScanAngle";
        public const string SCAN_CENTER_ANGLE = "ScanCenterAngle";
        public const string WAIT_TIME_ON_EDGE = "WaitTimeOnEdge";
    }
}