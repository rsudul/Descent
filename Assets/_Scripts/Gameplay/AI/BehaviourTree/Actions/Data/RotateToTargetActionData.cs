namespace Descent.Gameplay.AI.BehaviourTree.Actions.Data
{
    public class RotateToTargetActionData : IBehaviourTreeActionData
    {
        public float TargetYAngle { get; private set; }

        public RotateToTargetActionData(float targetYAngle)
        {
            TargetYAngle = targetYAngle;
        }
    }
}