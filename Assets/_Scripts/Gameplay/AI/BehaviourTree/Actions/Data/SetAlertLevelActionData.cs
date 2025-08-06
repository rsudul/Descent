using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Actions.Data
{
    public class SetAlertLevelActionData : IBehaviourTreeActionData
    {
        public AlertLevel TargetAlertLevel { get; }
        public bool ResetTimers { get; }
        public float SearchDuration { get; }
        public float CooldownDuration { get; }

        public SetAlertLevelActionData(AlertLevel targetAlertLevel, bool resetTimers,
            float searchDuration, float cooldownDuration)
        {
            TargetAlertLevel = targetAlertLevel;
            ResetTimers = resetTimers;
            SearchDuration = searchDuration;
            CooldownDuration = cooldownDuration;
        }
    }
}