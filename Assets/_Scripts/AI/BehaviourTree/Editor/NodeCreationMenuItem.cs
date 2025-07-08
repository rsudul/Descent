using System;
using Descent.AI.BehaviourTree.Nodes;

namespace Descent.AI.BehaviourTree.Editor
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