using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorOverlay(NodeInspectorOverlayType.WithCondition)]
    [NodeInspectorLabel("Repeat While Condition")]
    public class BehaviourTreeRepeatWhileConditionNode : BehaviourTreeCompositeNode
    {
        private int _currentChildIndex = 0;

        [SerializeField]
        [ConditionInvertField]
        private bool _invert = false;

        [SerializeReference]
        [ShowInNodeInspector("Condition")]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Condition == null)
            {
                ResetNode();
                Status = BehaviourTreeStatus.Failure;
                return BehaviourTreeStatus.Failure;
            }

            if (Children?.Count == 0)
            {
                Status = BehaviourTreeStatus.Success;
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

                if (status == BehaviourTreeStatus.Failure)
                {
                    ResetNode();
                    Status = BehaviourTreeStatus.Failure;
                    return BehaviourTreeStatus.Failure;
                }

                _currentChildIndex++;
            }

            ResetNode();

            Status = BehaviourTreeStatus.Running;
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

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeRepeatWhileConditionNode clone = ScriptableObject.CreateInstance<BehaviourTreeRepeatWhileConditionNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone._currentChildIndex = _currentChildIndex;
            clone._invert = _invert;
            clone.Condition = Condition?.Clone();
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
        }
    }
}