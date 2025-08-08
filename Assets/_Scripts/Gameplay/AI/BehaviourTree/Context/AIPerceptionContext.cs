using Descent.Gameplay.Systems.Perception;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;
using System.Collections.Generic;
using Descent.Gameplay.Entities;
using System.Linq;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIPerceptionContext : BehaviourTreeContext
    {
        private IPerceptionController PerceptionController = null;

        public Transform CurrentTarget { get; private set; }
        public IReadOnlyList<Actor> VisibleActors => PerceptionController?.VisibleActors.ToList() ?? null;

        public AIPerceptionContext(GameObject owner, IPerceptionController perceptionController, Transform currentTarget)
            : base(owner)
        {
            PerceptionController = perceptionController;
            CurrentTarget = currentTarget;
        }
    }
}