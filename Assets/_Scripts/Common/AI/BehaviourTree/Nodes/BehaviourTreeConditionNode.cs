using Descent.Common.AI.BehaviourTree.Core;
using System;
using UnityEngine;
using Descent.Common.AI.BehaviourTree.Core.Context;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [Serializable]
    public class BehaviourTreeConditionNode : BehaviourTreeNode
    {
        [SerializeField]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContext context)
        {
            return Condition?.Check(context) == true ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
    }
}