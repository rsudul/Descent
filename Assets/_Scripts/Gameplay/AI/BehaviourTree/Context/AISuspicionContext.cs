using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.Systems.Alert;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AISuspicionContext : BehaviourTreeContext
    {
        public ISuspicionController SuspicionController { get; private set; }

        public float SuspicionLevel => SuspicionController.SuspicionLevel;

        public AISuspicionContext(GameObject owner, ISuspicionController suspicionController) : base(owner)
        {
            SuspicionController = suspicionController;
        }
    }
}