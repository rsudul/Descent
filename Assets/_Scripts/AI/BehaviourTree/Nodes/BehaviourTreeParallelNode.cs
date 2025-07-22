using UnityEngine;
using Descent.Common.Attributes.AI;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
namespace Descent.AI.BehaviourTree.Nodes
{
    /// <summary>
    /// ---Parallel Node---
    /// 
    /// A composite node executing all its children in parallel.
    /// Failure / Success policies:
    ///  - FailOnAny: first child returns Failure -> Parallel node immediately returns Failure
    ///  - FailOnAll: Parallel node returns Failure only when all children returned Failure
    ///  - SucceedOnOne: if just one child returns Success, Parallel node also immediately returns Success
    ///  - SucceedOnAll: Parallel node returns Success only when all children returned Success
    /// In all other cases, Parallel node returns Running.
    /// </summary>
    [System.Serializable]
    [NodeInspectorLabel("Parallel")]
    public class BehaviourTreeParallelNode : BehaviourTreeCompositeNode
    {
        [SerializeField, Tooltip("Determine when parallel fails")]
        [ShowInNodeInspector("Failure policy")]
        private BehaviourTreeParallelFailurePolicy _failurePolicy = BehaviourTreeParallelFailurePolicy.FailOnAny;
        [SerializeField, Tooltip("Determine when parallel succeeds")]
        [ShowInNodeInspector("Success policy")]
        private BehaviourTreeParallelSuccessPolicy _successPolicy = BehaviourTreeParallelSuccessPolicy.SucceedOnAll;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            int successCount = 0;
            int failureCount = 0;

            foreach (BehaviourTreeNode child in Children)
            {
                BehaviourTreeStatus childStatus = child.Tick(contextRegistry);
                switch (childStatus)
                {
                    case BehaviourTreeStatus.Success:
                        successCount++;
                        break;

                    case BehaviourTreeStatus.Failure:
                        failureCount++;
                        break;
                }

                if (_failurePolicy == BehaviourTreeParallelFailurePolicy.FailOnAny
                    && childStatus == BehaviourTreeStatus.Failure)
                {
                    Status = BehaviourTreeStatus.Failure;
                    return Status;
                }

                if (_successPolicy == BehaviourTreeParallelSuccessPolicy.SucceedOnOne
                    && childStatus == BehaviourTreeStatus.Success)
                {
                    Status = BehaviourTreeStatus.Success;
                    return Status;
                }
            }

            if (_failurePolicy == BehaviourTreeParallelFailurePolicy.FailOnAll
                && failureCount == Children.Count)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            if (_successPolicy == BehaviourTreeParallelSuccessPolicy.SucceedOnAll
                && successCount == Children.Count)
            {
                Status = BehaviourTreeStatus.Success;
                return Status;
            }

            Status = BehaviourTreeStatus.Running;
            return Status;
        }

        public override void ResetNode()
        {
            if (Children != null && Children.Count > 0)
            {
                foreach (BehaviourTreeNode child in Children)
                {
                    child.ResetNode();
                }
            }

            Status = BehaviourTreeStatus.Running;
        }
    }
}