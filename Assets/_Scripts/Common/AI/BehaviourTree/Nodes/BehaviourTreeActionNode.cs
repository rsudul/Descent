using Descent.Common.AI.BehaviourTree.Core;
using System;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [Serializable]
    public class BehaviourTreeActionNode : BehaviourTreeNode
    {
        [SerializeReference]
        public IBehaviourTreeAction Action;

        public override BehaviourTreeStatus Update(BehaviourTreeContext context)
        {
            return Action?.Execute(context) ?? BehaviourTreeStatus.Failure;
        }
    }
}