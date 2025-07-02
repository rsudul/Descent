using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public class BehaviourTreeSequenceNode : BehaviourTreeCompositeNode
    {
        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            foreach (BehaviourTreeNode child in _children)
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