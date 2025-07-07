using Descent.Gameplay.Systems.Perception;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Context
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