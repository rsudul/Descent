using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    public class BehaviourTreeNodeEditorProxy : ScriptableObject
    {
        [SerializeReference]
        public BehaviourTreeNode Node;

        public static BehaviourTreeNodeEditorProxy Create(BehaviourTreeNode node)
        {
            BehaviourTreeNodeEditorProxy proxy = CreateInstance<BehaviourTreeNodeEditorProxy>();
            proxy.Node = node;
            return proxy;
        }
    }
}