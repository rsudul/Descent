using Descent.Common.AI.BehaviourTree.Core;
using System;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public class BehaviourTreeActionNode : BehaviourTreeNode
    {
        private readonly Func<BehaviourTreeContext, BehaviourTreeStatus> _action;

        public BehaviourTreeActionNode(Func<BehaviourTreeContext, BehaviourTreeStatus> action, string name = "Action")
        {
            _action = action;
            Name = name;
        }

        public override BehaviourTreeStatus Update(BehaviourTreeContext context) => _action(context);
    }
}