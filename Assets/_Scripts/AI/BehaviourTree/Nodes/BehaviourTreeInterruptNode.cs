using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    /// <summary>
    /// Interrupt decorator
    /// 
    /// When condition is true, stops child execution and returns Failure.
    /// Otherwise, executes the child.
    /// </summary>
    [System.Serializable]
    [NodeInspectorLabel("Interrupt")]
    public class BehaviourTreeInterruptNode : BehaviourTreeCompositeNode
    {
        [SerializeField, Tooltip("Condition that triggers interruption.")]
        [ShowInNodeInspector("Condition")]
        private IBehaviourTreeCondition Condition;

        public override void AddChild(BehaviourTreeNode child)
        {
            if (child == null)
            {
                throw new System.ArgumentNullException(nameof(child));
            }

            if (Children != null && Children.Count > 0)
            {
                throw new System.InvalidOperationException("Interrupt node may have only one child.");
            }

            base.AddChild(child);
        }

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Condition == null)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            if (Condition.Check(contextRegistry))
            {
                Children[0].ResetNode();
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            Status = Children[0].Tick(contextRegistry);
            return Status;
        }

        public override void ResetNode()
        {
            if (Children != null && Children.Count > 0)
            {
                Children[0].ResetNode();
            }
            Condition?.ResetCondition();
            Status = BehaviourTreeStatus.Running;
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeInterruptNode clone = ScriptableObject.CreateInstance<BehaviourTreeInterruptNode>();
            clone.Name = name;
            clone.Position = Position;
            clone.Condition = Condition?.Clone();
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
        }
    }
}