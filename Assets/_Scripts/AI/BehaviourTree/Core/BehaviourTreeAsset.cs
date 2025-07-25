using Descent.AI.BehaviourTree.Nodes;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Core
{
    [CreateAssetMenu(menuName = "Descent/AI/BehaviourTree")]
    public class BehaviourTreeAsset : ScriptableObject
    {
        [SerializeField]
        public BehaviourTreeNode Root;

        public BehaviourTreeNode CloneTree()
        {
            return Root.CloneNode();
        }
    }
}