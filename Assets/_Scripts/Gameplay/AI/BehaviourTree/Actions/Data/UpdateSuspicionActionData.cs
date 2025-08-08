namespace Descent.Gameplay.AI.BehaviourTree.Actions.Data
{
    public class UpdateSuspicionActionData : IBehaviourTreeActionData
    {
        public float IncreaseRate { get; }
        public float DecayRate { get; }
        public bool HasTarget { get; }

        public UpdateSuspicionActionData(float increaseRate, float decayRate, bool hasTarget)
        {
            IncreaseRate = increaseRate;
            DecayRate = decayRate;
            HasTarget = hasTarget;
        }
    }
}