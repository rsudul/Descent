using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    /// <summary>
    /// --Guard Decorator---
    /// 
    /// Executes child node only when condition is true.
    /// Otherwise returns Failure or Success (depending on the configuration).
    /// </summary>
    [System.Serializable]
    [NodeInspectorLabel("Guard")]
    public class BehaviourTreeGuardNode : BehaviourTreeCompositeNode
    {
        [SerializeField, SerializeReference, Tooltip("Condition that gates execution of the child.")]
        [ShowInNodeInspector("Condition")]
        public IBehaviourTreeCondition Condition;
        [SerializeField, Tooltip("If true, returns Success when condition fails; otherwise returns Failure.")]
        [ShowInNodeInspector("Success on fail")]
        private bool _successOnFail = false;

        public override void AddChild(BehaviourTreeNode child)
        {
            if (child == null)
            {
                throw new System.ArgumentNullException(nameof(child));
            }

            if (Children != null && Children.Count > 0)
            {
                throw new System.InvalidOperationException("Guard node may have only one child.");
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

            if (!Condition.Check(contextRegistry))
            {
                Status = _successOnFail ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
                return Status;
            }

            if (Children == null || Children.Count == 0)
            {
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
    }
}