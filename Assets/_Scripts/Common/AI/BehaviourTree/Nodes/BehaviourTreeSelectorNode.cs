using Descent.Common.AI.BehaviourTree.Core;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public class BehaviourTreeSelectorNode : BehaviourTreeCompositeNode
    {
        public override BehaviourTreeStatus Update(BehaviourTreeContext context)
        {
            foreach (BehaviourTreeNode child in _children)
            {
                BehaviourTreeStatus status = child.Update(context);
                if (status != BehaviourTreeStatus.Failure)
                {
                    return status;
                }
            }

            return BehaviourTreeStatus.Failure;
        }
    }
}