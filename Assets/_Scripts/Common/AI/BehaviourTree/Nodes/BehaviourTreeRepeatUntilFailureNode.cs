using Descent.Attributes.AI;
using Descent.Common.AI.BehaviourTree.Core;
using Descent.Common.AI.BehaviourTree.Context;
using UnityEngine;

namespace Descent.Common.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Repeat Until Failure")]
    public class BehaviourTreeRepeatUntilFailureNode : BehaviourTreeNode
    {
        private bool _isChildRunning = false;

        [SerializeField]
        public BehaviourTreeNode Child;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Child == null)
            {
                Status = BehaviourTreeStatus.Failure;
                return BehaviourTreeStatus.Failure;
            }

            BehaviourTreeStatus status = Child.Tick(contextRegistry);

            if (status == BehaviourTreeStatus.Running)
            {
                _isChildRunning = true;
                Status = BehaviourTreeStatus.Running;
                return BehaviourTreeStatus.Running;
            }

            if (status == BehaviourTreeStatus.Failure)
            {
                ResetNode();
                Status = BehaviourTreeStatus.Failure;
                return BehaviourTreeStatus.Failure;
            }

            if (_isChildRunning)
            {
                ResetNode();
            }

            Status = BehaviourTreeStatus.Running;
            return BehaviourTreeStatus.Running;
        }

        public override void ResetNode()
        {
            _isChildRunning = false;
            Child?.ResetNode();
        }
    }
}