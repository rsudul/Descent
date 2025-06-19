using Descent.Common.AI.BehaviourTree.Core;
using System;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [Serializable]
    public class BehaviourTreeConditionNode : BehaviourTreeNode
    {
        [SerializeReference]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Update(BehaviourTreeContext context)
        {
            return Condition?.Check(context) == true ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
    }
}