using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Repeat Until")]
    public class BehaviourTreeRepeatUntilNode : BehaviourTreeCompositeNode
    {
        private bool _isChildRunning = false;

        [ShowInNodeInspector("Repeat Until")]
        [SerializeField]
        private RepeatUntilType _condition = RepeatUntilType.Failure;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children?.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return BehaviourTreeStatus.Failure;
            }

            BehaviourTreeStatus childStatus = Children[0].Tick(contextRegistry);

            if (childStatus == BehaviourTreeStatus.Running)
            {
                _isChildRunning = true;
                Status = BehaviourTreeStatus.Running;
                return BehaviourTreeStatus.Running;
            }

            bool shouldStop = _condition switch
            {
                RepeatUntilType.Failure => childStatus == BehaviourTreeStatus.Failure,
                RepeatUntilType.Success => childStatus == BehaviourTreeStatus.Success,
                _ => false
            };

            if (shouldStop)
            {
                Status = childStatus;
                return Status;
            }

            if (_isChildRunning)
            {
                Children[0].ResetNode();
                _isChildRunning = false;
            }

            Status = BehaviourTreeStatus.Running;
            return Status;
        }

        public override void ResetNode()
        {
            _isChildRunning = false;
            if (Children.Count > 0)
            {
                Children[0].ResetNode();
            }
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeRepeatUntilNode clone = ScriptableObject.CreateInstance<BehaviourTreeRepeatUntilNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone._isChildRunning = _isChildRunning;
            clone._condition = _condition;
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
        }
    }
}