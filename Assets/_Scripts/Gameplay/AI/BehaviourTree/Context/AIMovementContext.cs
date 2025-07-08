using Descent.Gameplay.Movement;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIMovementContext : BehaviourTreeContext
    {
        public Vector3? TargetPosition { get; set; }
        public IMovementController MovementController { get; private set; }

        public bool IsMoving => MovementController?.IsMoving ?? false;

        public AIMovementContext(GameObject owner, IMovementController movementController) : base(owner)
        {
            MovementController = movementController;
        }
    }
}