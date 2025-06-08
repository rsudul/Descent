using Descent.Common.AI.BehaviourTree.Core;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public abstract class BehaviourTreeNode
    {
        public string Name { get; set; }
        public BehaviourTreeNode Parent { get; set; }

        public virtual void Initialize(BehaviourTreeContext context)
        {

        }

        public abstract BehaviourTreeStatus Update(BehaviourTreeContext context);
    }
}