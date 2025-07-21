using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Priority Reactive Selector")]
    public class BehaviourTreePriorityReactiveSelector : BehaviourTreeCompositeNode
    {
        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            foreach (BehaviourTreeNode child in Children)
            {
                BehaviourTreeStatus childStatus = child.Tick(contextRegistry);
                if (childStatus == BehaviourTreeStatus.Running)
                {
                    Status = BehaviourTreeStatus.Running;
                    return Status;
                }
                if (childStatus == BehaviourTreeStatus.Success)
                {
                    Status = BehaviourTreeStatus.Success;
                    return Status;
                }
                child.ResetNode();
            }

            Status = BehaviourTreeStatus.Failure;
            return Status;
        }

        public override void ResetNode()
        {
            if (Children != null && Children.Count > 0)
            {
                foreach (BehaviourTreeNode child in Children)
                {
                    child.ResetNode();
                }
            }

            Status = BehaviourTreeStatus.Running;
        }
    }
}