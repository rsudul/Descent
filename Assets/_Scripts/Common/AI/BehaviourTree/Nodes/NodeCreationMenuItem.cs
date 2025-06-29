using System;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public class NodeCreationMenuItem
    {
        public string DisplayName;
        public Func<BehaviourTreeNode> CreateFunc;

        public NodeCreationMenuItem(string displayName, Func<BehaviourTreeNode> createFunc)
        {
            DisplayName = displayName;
            CreateFunc = createFunc;
        }
    }
}