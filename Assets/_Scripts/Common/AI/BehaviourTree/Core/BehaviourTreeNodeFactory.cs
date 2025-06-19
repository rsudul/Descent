using UnityEngine;
using Descent.Common.AI.BehaviourTree.Nodes;

namespace Descent.Common.AI.BehaviourTree.Core {
    public static class BehaviourTreeNodeFactory
    {
        public static BehaviourTreeNode Create(System.Type type)
        {
            if (type == typeof(BehaviourTreeActionNode))
            {
                return new BehaviourTreeActionNode();
            }

            if (type == typeof(BehaviourTreeConditionNode))
            {
                return new BehaviourTreeConditionNode();
            }

            if (type == typeof(BehaviourTreeSelectorNode))
            {
                return new BehaviourTreeSelectorNode();
            }

            if (type == typeof(BehaviourTreeSequenceNode))
            {
                return new BehaviourTreeSequenceNode();
            }

            Debug.LogError($"Cannot create node of type: {type}");
            return null;
        }
    }
}