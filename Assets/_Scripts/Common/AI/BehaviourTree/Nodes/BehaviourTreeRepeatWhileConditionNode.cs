using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public class BehaviourTreeRepeatWhileConditionNode : BehaviourTreeCompositeNode
    {
        private int _currentChildIndex = 0;

        [SerializeReference]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Condition == null || !Condition.Check(contextRegistry))
            {
                ResetNode();
                return BehaviourTreeStatus.Failure;
            }

            if (Children?.Count == 0)
            {
                return BehaviourTreeStatus.Success;
            }

            while (_currentChildIndex < Children.Count)
            {
                BehaviourTreeStatus status = Children[_currentChildIndex].Tick(contextRegistry);

                if (status == BehaviourTreeStatus.Running)
                {
                    return BehaviourTreeStatus.Running;
                }

                if (status == BehaviourTreeStatus.Failure)
                {
                    ResetNode();
                    return BehaviourTreeStatus.Failure;
                }

                _currentChildIndex++;
            }

            ResetNode();

            return BehaviourTreeStatus.Running;
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