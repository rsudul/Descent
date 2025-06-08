using System.Collections.Generic;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public abstract class BehaviourTreeCompositeNode : BehaviourTreeNode
    {
        protected List<BehaviourTreeNode> _children = new List<BehaviourTreeNode>();

        public void AddChild(BehaviourTreeNode child)
        {
            _children.Add(child);
            child.Parent = this;
        }

        public void SetChildren(IEnumerable<BehaviourTreeNode> children)
        {
            _children.Clear();
            foreach (BehaviourTreeNode child in children)
            {
                AddChild(child);
            }
        }
    }
}