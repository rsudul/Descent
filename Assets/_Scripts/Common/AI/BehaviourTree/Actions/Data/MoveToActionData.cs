using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Actions.Data
{
    public class MoveToActionData : IBehaviourTreeActionData
    {
        public Vector3 Target;
        public float Speed;

        public MoveToActionData(Vector3 target, float speed)
        {
            Target = target;
            Speed = speed;
        }
    }
}