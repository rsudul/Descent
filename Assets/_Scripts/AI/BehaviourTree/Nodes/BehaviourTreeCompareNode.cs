using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Compare")]
    public class BehaviourTreeCompareNode : BehaviourTreeNode
    {
        [SerializeField, Tooltip("Blackboard key")]
        private string _key;
        [SerializeField, Tooltip("Comparison type")]
        private BehaviourTreeCompareOperation _operation;
        [SerializeField, Tooltip("Threshold value")]
        private float _threshold;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            float actual = contextRegistry.Blackboard.Get<float>(_key, 0.0f);
            bool ok = false;
            
            switch (_operation)
            {
                case BehaviourTreeCompareOperation.Equal:
                    ok = Mathf.Approximately(actual, _threshold);
                    break;

                case BehaviourTreeCompareOperation.NotEqual:
                    ok = !Mathf.Approximately(actual, _threshold);
                    break;

                case BehaviourTreeCompareOperation.Less:
                    ok = actual < _threshold;
                    break;

                case BehaviourTreeCompareOperation.LessOrEqual:
                    ok = actual <= _threshold;
                    break;

                case BehaviourTreeCompareOperation.Greater:
                    ok = actual > _threshold;
                    break;

                case BehaviourTreeCompareOperation.GreaterOrEqual:
                    ok = actual >= _threshold;
                    break;

                default:
                    ok = false;
                    break;
            }

            Status = ok ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
            return Status;
        }

        public override void ResetNode()
        {
            Status = BehaviourTreeStatus.Running;
        }
    }
}