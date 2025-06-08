using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Core
{
    public class BehaviourTreeContext
    {
        public GameObject Owner { get; }
        public Transform OwnerTransform => Owner.transform;

        public BehaviourTreeContext(GameObject owner)
        {
            Owner = owner;
        }
    }
}