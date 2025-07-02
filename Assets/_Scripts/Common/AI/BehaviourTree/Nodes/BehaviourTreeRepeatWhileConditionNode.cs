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
            while (Condition?.Check(contextRegistry) == true)
            {
                foreach (var child in Children)
                {
                    var status = child.Tick(contextRegistry);
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