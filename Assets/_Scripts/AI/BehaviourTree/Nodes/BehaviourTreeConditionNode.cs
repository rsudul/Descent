using Descent.AI.BehaviourTree.Core;
using System;
using UnityEngine;
using Descent.AI.BehaviourTree.Context;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Conditions;

namespace Descent.AI.BehaviourTree.Nodes
{
    [Serializable]
    [NodeInspectorOverlay(NodeInspectorOverlayType.WithCondition)]
    [NodeInspectorLabel("Conditional")]
    public class BehaviourTreeConditionNode : BehaviourTreeNode
    {
        [SerializeField]
        [ShowInNodeInspector("Condition")]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            BehaviourTreeStatus status = Condition?.Check(contextRegistry) == true
                ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
            Status = status;
            return status;
        }

        public override void ResetNode()
        {

        }
    }
}