using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public abstract class BehaviourTreeNode
    {
        [SerializeField]
        public string Name = "Node";
        public BehaviourTreeNode Parent { get; set; }

        public virtual void Initialize(BehaviourTreeContext context)
        {

        }

        public abstract BehaviourTreeStatus Update(BehaviourTreeContext context);
    }
}