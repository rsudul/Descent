using Descent.Gameplay.Movement;
using UnityEngine;

namespace Descent.Gameplay.AI.BehaviourTree.Context
{
    public class AIScanContext : AIRotationContext
    {
        public float CenterAngle { get; private set; } = 0.0f;
        public float ScanAngle { get; private set; } = 0.0f;
        public float WaitTimeOnEdge { get; private set; } = 0.0f;

        public AIScanContext(GameObject owner, IAIRotationController rotationController,
            float centerAngle, float scanAngle, float waitTimeOnEdge) : base(owner, rotationController)
        {
            CenterAngle = centerAngle;
            ScanAngle = scanAngle;
            WaitTimeOnEdge = waitTimeOnEdge;
        }
    }
}