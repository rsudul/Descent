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
        [ShowInNodeInspector("Mode")]
        [SerializeField]
        private ParallelMode _mode = ParallelMode.RequireOne;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            int successCount = 0;
            int failureCount = 0;
            int runningCount = 0;

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

                    case BehaviourTreeStatus.Running:
                        runningCount++;
                        break;
                }
            }

            switch (_mode)
            {
                case ParallelMode.RequireOne:
                    if (successCount > 0)
                    {
                        Status = BehaviourTreeStatus.Success;
                        return Status;
                    }
                    if (failureCount == Children.Count)
                    {
                        Status = BehaviourTreeStatus.Failure;
                        return Status;
                    }
                    break;

                case ParallelMode.RequireAll:
                    if (successCount == Children.Count)
                    {
                        Status = BehaviourTreeStatus.Success;
                        return Status;
                    }
                    if (failureCount > 0)
                    {
                        Status = BehaviourTreeStatus.Failure;
                        return Status;
                    }
                    break;
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

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeParallelNode clone = ScriptableObject.CreateInstance<BehaviourTreeParallelNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone._mode = _mode;
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
        }
    }
}