using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Attributes.AI;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public abstract class BehaviourTreeNode : ScriptableObject
    {
        [SerializeField]
        [ShowInNodeInspector("Name", 1000)]
        public string Name = "Node";
        [SerializeField]
        public BehaviourTreeNode Parent { get; set; }
        [SerializeField]
        public Vector2 Position { get; set; }

        public abstract BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry);

        public abstract void ResetNode();
    }
}