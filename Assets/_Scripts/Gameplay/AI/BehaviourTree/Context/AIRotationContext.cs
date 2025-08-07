using Descent.AI.BehaviourTree.Context;
using Descent.Gameplay.Movement;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIRotationContext : BehaviourTreeContext
    {
        private IAIRotationController RotationController = null;

        public bool IsRotating => RotationController?.IsRotating ?? false;
        public float CurrentYAngle => RotationController?.CurrentYAngle ?? 0.0f;

        public AIRotationContext(GameObject owner, IAIRotationController rotationController) : base(owner)
        {
            RotationController = rotationController;
        }
    }
}