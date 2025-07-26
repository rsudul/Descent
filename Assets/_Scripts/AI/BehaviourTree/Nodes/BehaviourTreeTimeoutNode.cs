using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    /// <summary>
    /// Timeout decorator
    /// 
    /// Stops child execution if it does not finish in the specified time.
    /// </summary>
    [System.Serializable]
    [NodeInspectorLabel("Timeout")]
    public class BehaviourTreeTimeoutNode : BehaviourTreeCompositeNode
    {
        private float _startTime = 0.0f;
        private bool _timerStarted = false;

        [SerializeField, Tooltip("Time limit in seconds after which child is aborted.")]
        [ShowInNodeInspector("Time limit")]
        private float _timeLimit = 1.0f;

        public override void AddChild(BehaviourTreeNode child)
        {
            if (child == null)
            {
                throw new System.ArgumentNullException(nameof(child));
            }

            if (Children != null && Children.Count > 1)
            {
                throw new System.InvalidOperationException("Timeout node may have only one child.");
            }

            base.AddChild(child);
        }

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            BehaviourTreeNode child = Children[0];

            if (!_timerStarted)
            {
                _startTime = Time.time;
                _timerStarted = true;
            }

            if (Time.time - _startTime > _timeLimit)
            {
                child.ResetNode();
                _timerStarted = false;
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            BehaviourTreeStatus childStatus = child.Tick(contextRegistry);

            if (childStatus != BehaviourTreeStatus.Running)
            {
                _timerStarted = false;
            }

            Status = childStatus;
            return Status;
        }

        public override void ResetNode()
        {
            if (Children != null && Children.Count > 0)
            {
                Children[0].ResetNode();
            }

            _timerStarted = false;
            _startTime = 0.0f;
            Status = BehaviourTreeStatus.Running;
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeTimeoutNode clone = ScriptableObject.CreateInstance<BehaviourTreeTimeoutNode>();
            clone.Name = name;
            clone.Position = Position;
            clone._startTime = _startTime;
            clone._timerStarted = _timerStarted;
            clone._timeLimit = _timeLimit;
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
        }
    }
}