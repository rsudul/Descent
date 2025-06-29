using Descent.Common.AI.BehaviourTree.Nodes;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Core
{
    [CreateAssetMenu(menuName = "Descent/AI/BehaviourTree")]
    public class BehaviourTreeAsset : ScriptableObject
    {
        [SerializeField]
        public BehaviourTreeNode Root;

        public BehaviourTreeNode CloneTree()
        {
            var cloneMap = new Dictionary<BehaviourTreeNode, BehaviourTreeNode>();
            return CloneNodeRecursive(Root, cloneMap);
        }

        private BehaviourTreeNode CloneNodeRecursive(BehaviourTreeNode original, Dictionary<BehaviourTreeNode, BehaviourTreeNode> map)
        {
            if (original == null)
            {
                return null;
            }

            if (map.TryGetValue(original, out var existingClone))
            {
                return existingClone;
            }

            BehaviourTreeNode clone = Object.Instantiate(original);
            map[original] = clone;

            if (clone is BehaviourTreeCompositeNode compositeOriginal
                && original is BehaviourTreeCompositeNode compositeSource)
            {
                foreach (BehaviourTreeNode child in compositeSource.Children)
                {
                    var childClone = CloneNodeRecursive(child, map);
                    compositeOriginal.AddChild(childClone);
                }
            }
            else if (clone is BehaviourTreeConditionNode conditionClone
                && original is BehaviourTreeConditionNode conditionOriginal)
            {
                conditionClone.Condition = conditionOriginal.Condition?.Clone();
            }
            else if (clone is BehaviourTreeActionNode actionClone
                && original is BehaviourTreeActionNode actionOriginal)
            {
                actionClone.Action = actionOriginal.Action?.Clone();
            }

            return clone;
        }
    }
}