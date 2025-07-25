using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Sequence")]
    public class BehaviourTreeSequenceNode : BehaviourTreeCompositeNode
    {
        private int _currentChildIndex = 0;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children?.Count == 0)
            {
                Status = BehaviourTreeStatus.Success;
                return BehaviourTreeStatus.Success;
            }

            while (_currentChildIndex < Children.Count)
            {
                BehaviourTreeStatus status = Children[_currentChildIndex].Tick(contextRegistry);

                if (status == BehaviourTreeStatus.Running)
                {
                    Status = BehaviourTreeStatus.Running;
                    return BehaviourTreeStatus.Running;
                }

                if (status == BehaviourTreeStatus.Failure)
                {
                    ResetNode();
                    Status = BehaviourTreeStatus.Failure;
                    return BehaviourTreeStatus.Failure;
                }

                _currentChildIndex++;
            }

            ResetNode();
            Status = BehaviourTreeStatus.Success;
            return BehaviourTreeStatus.Success;
        }

        public override void ResetNode()
        {
            _currentChildIndex = 0;

            if (Children?.Count == 0)
            {
                return;
            }

            foreach (BehaviourTreeNode child in Children)
            {
                child.ResetNode();
            }
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeSequenceNode clone = ScriptableObject.CreateInstance<BehaviourTreeSequenceNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone._currentChildIndex = _currentChildIndex;
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
        }
    }
}