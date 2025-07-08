using System.Collections.Generic;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public abstract class BehaviourTreeCompositeNode : BehaviourTreeNode
    {
        [SerializeField]
        protected List<BehaviourTreeNode> _children = new List<BehaviourTreeNode>();
        public IReadOnlyList<BehaviourTreeNode> Children => _children.AsReadOnly();

        public void AddChild(BehaviourTreeNode child)
        {
            if (child == null || _children.Contains(child) || child.Parent != null)
            {
                return;
            }

            _children.Add(child);
            child.Parent = this;
        }

        public void RemoveChild(BehaviourTreeNode child)
        {
            if (!_children.Contains(child))
            {
                return;
            }
            _children.Remove(child);

            if (child.Parent == this)
            {
                child.Parent = null;
            }
        }
    }
}