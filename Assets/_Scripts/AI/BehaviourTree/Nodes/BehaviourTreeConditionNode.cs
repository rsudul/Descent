using Descent.AI.BehaviourTree.Core;
using System;
using Descent.AI.BehaviourTree.Context;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Conditions;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    [Serializable]
    [NodeInspectorLabel("Conditional")]
    public class BehaviourTreeConditionNode : BehaviourTreeNode
    {
        [SerializeField, SerializeReference]
        [ShowInNodeInspector("Condition")]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Condition == null)
            {
                Debug.LogError($"[{nameof(BehaviourTreeConditionNode)}] Condition is null on node {Name}");
                return BehaviourTreeStatus.Failure;
            }
            BehaviourTreeStatus status = Condition.Check(contextRegistry)
                                        ? BehaviourTreeStatus.Success
                                        : BehaviourTreeStatus.Failure;

            Status = status;
            return status;
        }

        public override void ResetNode()
        {
            Condition?.ResetCondition();
            Status = BehaviourTreeStatus.Running;
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeConditionNode clone = ScriptableObject.CreateInstance<BehaviourTreeConditionNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone.Condition = Condition?.Clone();
            return clone;
        }
    }
}