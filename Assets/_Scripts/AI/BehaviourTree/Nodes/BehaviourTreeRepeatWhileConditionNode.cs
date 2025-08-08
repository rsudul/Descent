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
        [SerializeField]
        [ConditionInvertField]
        private bool _invert = false;

        [SerializeReference]
        [ShowInNodeInspector("Condition")]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children?.Count == 0)
            {
                return Status = BehaviourTreeStatus.Failure;
            }

            bool cond = Condition == null ? true : Condition.Check(contextRegistry);

            if (_invert)
            {
                cond = !cond;
            }

            if (!cond)
            {
                Children[0].ResetNode();
                return Status = BehaviourTreeStatus.Success;
            }

            BehaviourTreeStatus childStatus = Children[0].Tick(contextRegistry);

            if (childStatus == BehaviourTreeStatus.Success || childStatus == BehaviourTreeStatus.Failure)
            {
                Children[0].ResetNode();
            }

            return Status = BehaviourTreeStatus.Running;
        }

        public override void ResetNode()
        {
            if (Children?.Count > 0)
            {
                Children[0].ResetNode();
            }

            Condition?.ResetCondition();

            Status = BehaviourTreeStatus.Running;
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeRepeatWhileConditionNode clone = ScriptableObject.CreateInstance<BehaviourTreeRepeatWhileConditionNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone._invert = _invert;
            clone.Condition = Condition?.Clone();
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
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