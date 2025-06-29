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

        public override BehaviourTreeStatus Tick(BehaviourTreeContext context)
        {
            while (Condition?.Check(context) == true)
            {
                foreach (var child in Children)
                {
                    var status = child.Tick(context);
                    if (status != BehaviourTreeStatus.Success)
                    {
                        return status;
                    }
                }
            }

            return BehaviourTreeStatus.Success;
        }
    }
}