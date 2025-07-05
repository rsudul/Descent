using Descent.Attributes.AI;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorOverlay(NodeInspectorOverlayType.WithCondition)]
    public class BehaviourTreeRepeatWhileConditionNode : BehaviourTreeCompositeNode
    {
        private int _currentChildIndex = 0;

        [SerializeField]
        private bool _invert = false;

        [SerializeReference]
        [ShowInNodeInspector("Condition")]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Condition == null)
            {
                ResetNode();
                return BehaviourTreeStatus.Failure;
            }

            if (Children?.Count == 0)
            {
                return BehaviourTreeStatus.Success;
            }

            bool cond = Condition.Check(contextRegistry);
            if (_invert)
            {
                cond = !cond;
            }

            if (!cond)
            {
                ResetNode();
                return BehaviourTreeStatus.Failure;
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