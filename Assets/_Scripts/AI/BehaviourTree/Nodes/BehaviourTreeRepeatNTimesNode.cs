using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    /// <summary>
    /// ---Repeat n times---
    /// 
    /// Executes its children n times, with a specified time interval between each execution.
    /// Returns Running for as long as children execution is being repeated.
    /// After the last repeat and execution, returns Success.
    /// </summary>
    public class BehaviourTreeRepeatNTimesNode : BehaviourTreeCompositeNode
    {
        private int _executed = 0;
        private float _timer = 0.0f;

        private int _currentChildIndex = -1;

        [ShowInNodeInspector("Repeat count")]
        [SerializeField]
        private int _repeatCount = 1;
        [ShowInNodeInspector("Time interval")]
        [SerializeField]
        private float _interval = 0.0f;
        [ShowInNodeInspector("Failure policy")]
        [SerializeField]
        private BehaviourTreeFailurePolicy _failurePolicy = BehaviourTreeFailurePolicy.OnChildFailure;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            if (_executed >= _repeatCount)
            {
                Status = BehaviourTreeStatus.Success;
                return Status;
            }

            if (_timer > 0.0f)
            {
                _timer -= Time.deltaTime;
                Status = BehaviourTreeStatus.Running;
                return Status;
            }

            while (_currentChildIndex < Children.Count)
            {
                BehaviourTreeStatus childStatus = Children[_currentChildIndex].Tick(contextRegistry);

                if (childStatus == BehaviourTreeStatus.Failure && _failurePolicy == BehaviourTreeFailurePolicy.OnChildFailure)
                {
                    Status = BehaviourTreeStatus.Failure;
                    return Status;
                }

                if (childStatus == BehaviourTreeStatus.Success && _failurePolicy == BehaviourTreeFailurePolicy.OnChildSuccess)
                {
                    Status = BehaviourTreeStatus.Failure;
                    return Status;
                }

                if (childStatus == BehaviourTreeStatus.Running)
                {
                    Status = BehaviourTreeStatus.Running;
                    return Status;
                }
            }

            _executed++;

            if (_executed >= _repeatCount)
            {
                ResetNode();
                Status = BehaviourTreeStatus.Success;
                return Status;
            }

            _timer = _interval;
            Status = BehaviourTreeStatus.Running;
            return Status;
        }

        public override void ResetNode()
        {
            _executed = 0;
            _timer = 0.0f;
            if (Children != null && Children.Count > 0)
            {
                foreach (BehaviourTreeNode child in Children)
                {
                    child.ResetNode();
                }
            }
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeRepeatNTimesNode clone = ScriptableObject.CreateInstance<BehaviourTreeRepeatNTimesNode>();
            clone.Name = Name;
            clone._repeatCount = _repeatCount;
            clone._interval = _interval;
            if (Children != null && Children.Count > 0)
            {
                foreach (BehaviourTreeNode child in Children)
                {
                    clone.AddChild(child.CloneNode());
                }
            }
            return clone;
        }
    }
}