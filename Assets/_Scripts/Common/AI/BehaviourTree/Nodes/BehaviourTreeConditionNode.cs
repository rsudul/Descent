using Descent.Common.AI.BehaviourTree.Core;
using System;
using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core.Context;
using Descent.Attributes.AI;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [Serializable]
    [NodeInspectorOverlay(NodeInspectorOverlayType.WithCondition)]
    public class BehaviourTreeConditionNode : BehaviourTreeNode
    {
        [SerializeField]
        [ShowInNodeInspector("Condition")]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            return Condition?.Check(contextRegistry) == true ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }

        public override void ResetNode()
        {

        }
    }
}