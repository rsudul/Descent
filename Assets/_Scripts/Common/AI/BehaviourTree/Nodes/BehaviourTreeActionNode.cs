using Descent.Common.AI.BehaviourTree.Core;
using System;
using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core.Context;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [Serializable]
    public class BehaviourTreeActionNode : BehaviourTreeNode
    {
        [SerializeReference]
        public IBehaviourTreeAction Action;

        public override BehaviourTreeStatus Tick(BehaviourTreeContext context)
        {
            return Action?.Execute(context) ?? BehaviourTreeStatus.Failure;
        }
    }
}