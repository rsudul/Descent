using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Context;

namespace Descent.Gameplay.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public class AlwaysTrueCondition : IBehaviourTreeCondition
    {
        public bool Check(BehaviourTreeContextRegistry contextRegistry)
        {
            return true;
        }

        public void ResetCondition()
        {

        }

        public IBehaviourTreeCondition Clone()
        {
            return new AlwaysTrueCondition();
        }
    }
}