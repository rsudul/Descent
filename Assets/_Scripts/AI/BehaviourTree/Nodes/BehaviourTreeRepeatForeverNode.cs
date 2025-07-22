using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Repeat Forever")]
    public class BehaviourTreeRepeatForeverNode : BehaviourTreeCompositeNode
    {
        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            BehaviourTreeNode child = Children[0];
            BehaviourTreeStatus childStatus = child.Tick(contextRegistry);

            if (childStatus == BehaviourTreeStatus.Running)
            {
                Status = BehaviourTreeStatus.Running;
            }
            else
            {
                child.ResetNode();
                Status = BehaviourTreeStatus.Running;
            }

            return Status;
        }

        public override void ResetNode()
        {
            if (Children != null && Children.Count > 0)
            {
                foreach (BehaviourTreeNode child in Children)
                {
                    child?.ResetNode();
                }
            }

            Status = BehaviourTreeStatus.Running;
        }

        public override void AddChild(BehaviourTreeNode child)
        {
            if (child == null)
            {
                return;
            }

            if (Children != null && Children.Count > 0)
            {
                return;
            }

            base.AddChild(child);
        }
    }
}