using System;
using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Context;
using Descent.Attributes.AI;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [Serializable]
    [NodeInspectorLabel("Action")]
    public class BehaviourTreeActionNode : BehaviourTreeNode
    {
        [SerializeReference]
        [ShowInNodeInspector("Action")]
        public IBehaviourTreeAction Action;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            BehaviourTreeStatus status = Action?.Execute(contextRegistry) ?? BehaviourTreeStatus.Failure;
            Status = status;
            return status;
        }

        public override void ResetNode()
        {
            Action?.ResetAction();
        }
    }
}