using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Actions.Data
{
    public class EscalateAlertActionData : IBehaviourTreeActionData
    {
        public AlertLevel TargetLevel { get; }

        public EscalateAlertActionData(AlertLevel targetLevel)
        {
            TargetLevel = targetLevel;
        }
    }
}