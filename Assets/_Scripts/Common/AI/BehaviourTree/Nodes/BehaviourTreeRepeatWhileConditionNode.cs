using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public class BehaviourTreeRepeatWhileConditionNode : BehaviourTreeCompositeNode
    {
        [SerializeReference]
        public IBehaviourTreeCondition Condition;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Condition == null || !Condition.Check(contextRegistry))
            {
                return BehaviourTreeStatus.Failure;
            }

            foreach (BehaviourTreeNode child in Children)
            {
                BehaviourTreeStatus status = child.Tick(contextRegistry);
                if (status != BehaviourTreeStatus.Success)
                {
                    return status;
                }
            }

            return BehaviourTreeStatus.Success;
        }
    }
}