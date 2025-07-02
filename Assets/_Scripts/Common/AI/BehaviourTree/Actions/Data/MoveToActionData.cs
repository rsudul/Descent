using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Actions.Data
{
    public class MoveToActionData : IBehaviourTreeActionData
    {
        public Vector3 Target;

        public MoveToActionData(Vector3 target)
        {
            Target = target;
        }
    }
}