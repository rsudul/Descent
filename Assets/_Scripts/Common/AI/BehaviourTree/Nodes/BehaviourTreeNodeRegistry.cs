using System.Collections.Generic;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public static class BehaviourTreeNodeRegistry
    {
        public static readonly List<NodeCreationMenuItem> NodeTypes = new()
        {
            new NodeCreationMenuItem("Composite/Selector", () => new BehaviourTreeSelectorNode { Name = "Selector" }),
            new NodeCreationMenuItem("Composite/Sequence", () => new BehaviourTreeSequenceNode { Name = "Sequence" })
        };
    }
}