using Descent.Common.AI.BehaviourTree.Nodes;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Core
{
    [CreateAssetMenu(menuName = "Descent/AI/BehaviourTree")]
    public class BehaviourTreeAsset : ScriptableObject
    {
        [SerializeReference]
        public BehaviourTreeNode Root;
    }
}