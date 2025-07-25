using Descent.Gameplay.Systems.Perception;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIPerceptionContext : BehaviourTreeContext
    {
        public IPerceptionController PerceptionController { get; private set; }
        public Transform CurrentTarget { get; private set; }

        public AIPerceptionContext(GameObject owner, IPerceptionController perceptionController, Transform currentTarget)
            : base(owner)
        {
            PerceptionController = perceptionController;
            CurrentTarget = currentTarget;
        }
    }
}