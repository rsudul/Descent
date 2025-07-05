using Descent.Attributes.AI;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Context;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Selector")]
    public class BehaviourTreeSelectorNode : BehaviourTreeCompositeNode
    {
        private int _currentChildIndex = 0;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children?.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return BehaviourTreeStatus.Failure;
            }

            while (_currentChildIndex < Children.Count)
            {
                BehaviourTreeStatus status = Children[_currentChildIndex].Tick(contextRegistry);

                if (status == BehaviourTreeStatus.Running)
                {
                    Status = BehaviourTreeStatus.Running;
                    return BehaviourTreeStatus.Running;
                }

                if (status == BehaviourTreeStatus.Success)
                {
                    ResetNode();
                    Status = BehaviourTreeStatus.Success;
                    return BehaviourTreeStatus.Success;
                }

                _currentChildIndex++;
            }

            ResetNode();
            Status = BehaviourTreeStatus.Failure;
            return BehaviourTreeStatus.Failure;
        }

        public override void ResetNode()
        {
            _currentChildIndex = 0;

            if (Children?.Count == 0)
            {
                return;
            }

            foreach (BehaviourTreeNode child in Children)
            {
                child.ResetNode();
            }
        }
    }
}