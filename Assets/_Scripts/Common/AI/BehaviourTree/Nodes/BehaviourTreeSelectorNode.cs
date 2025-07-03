using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public class BehaviourTreeSelectorNode : BehaviourTreeCompositeNode
    {
        private int _currentChildIndex = 0;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children?.Count == 0)
            {
                return BehaviourTreeStatus.Failure;
            }

            while (_currentChildIndex < Children.Count)
            {
                BehaviourTreeStatus status = Children[_currentChildIndex].Tick(contextRegistry);

                if (status == BehaviourTreeStatus.Running)
                {
                    return BehaviourTreeStatus.Running;
                }

                if (status == BehaviourTreeStatus.Success)
                {
                    ResetNode();
                    return BehaviourTreeStatus.Success;
                }

                _currentChildIndex++;
            }

            ResetNode();
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