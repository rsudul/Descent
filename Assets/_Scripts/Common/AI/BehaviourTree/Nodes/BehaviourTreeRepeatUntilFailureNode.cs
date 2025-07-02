using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Core.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    public class BehaviourTreeRepeatUntilFailureNode : BehaviourTreeNode
    {
        [SerializeField]
        public BehaviourTreeNode Child;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Child == null)
            {
                return BehaviourTreeStatus.Failure;
            }

            while (true)
            {
                BehaviourTreeStatus status = Child.Tick(contextRegistry);
                if (status == BehaviourTreeStatus.Failure)
                {
                    return BehaviourTreeStatus.Failure;
                }
            }
        }
    }
}