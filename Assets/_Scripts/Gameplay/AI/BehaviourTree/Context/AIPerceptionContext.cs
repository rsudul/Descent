using Descent.Gameplay.Systems.Perception;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIPerceptionContext : BehaviourTreeContext
    {
        public IPerceptionController PerceptionController { get; private set; }

        public AIPerceptionContext(GameObject owner, IPerceptionController perceptionController) : base(owner)
        {
            PerceptionController = perceptionController;
        }
    }
}