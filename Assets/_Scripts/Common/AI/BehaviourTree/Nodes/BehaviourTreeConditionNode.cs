using Descent.Common.AI.BehaviourTree.Core;
using System;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public class BehaviourTreeConditionNode : BehaviourTreeNode
    {
        private readonly Func<BehaviourTreeContext, bool> _condition;

        public BehaviourTreeConditionNode(Func<BehaviourTreeContext, bool> condition, string name = "Condition")
        {
            _condition = condition;
            Name = name;
        }

        public override BehaviourTreeStatus Update(BehaviourTreeContext context)
        {
            return _condition(context) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
    }
}