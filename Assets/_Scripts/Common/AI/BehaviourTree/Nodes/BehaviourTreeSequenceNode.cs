using Descent.Common.AI.BehaviourTree.Core;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public class BehaviourTreeSequenceNode : BehaviourTreeCompositeNode
    {
        public override BehaviourTreeStatus Update(BehaviourTreeContext context)
        {
            foreach (BehaviourTreeNode child in _children)
            {
                BehaviourTreeStatus status = child.Update(context);
                if (status != BehaviourTreeStatus.Success)
                {
                    return status;
                }
            }

            return BehaviourTreeStatus.Success;
        }
    }
}