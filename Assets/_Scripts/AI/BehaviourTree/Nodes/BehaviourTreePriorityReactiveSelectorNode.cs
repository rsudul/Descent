using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    [System.Serializable]
    [NodeInspectorLabel("Priority Reactive Selector")]
    public class BehaviourTreePriorityReactiveSelector : BehaviourTreeCompositeNode
    {
        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            foreach (BehaviourTreeNode child in Children)
            {
                BehaviourTreeStatus childStatus = child.Tick(contextRegistry);
                if (childStatus == BehaviourTreeStatus.Running)
                {
                    ResetOtherChildren(child);
                    Status = BehaviourTreeStatus.Running;
                    return Status;
                }
                if (childStatus == BehaviourTreeStatus.Success)
                {
                    ResetOtherChildren(child);
                    Status = BehaviourTreeStatus.Success;
                    return Status;
                }
            }

            Status = BehaviourTreeStatus.Failure;
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
            BehaviourTreePriorityReactiveSelector clone = ScriptableObject.CreateInstance<BehaviourTreePriorityReactiveSelector>();
            clone.Name = Name;
            clone.Position = Position;
            foreach (BehaviourTreeNode child in Children)
            {
                clone.AddChild(child.CloneNode());
            }
            return clone;
        }

        private void ResetOtherChildren(BehaviourTreeNode activeChild)
        {
            foreach (BehaviourTreeNode child in Children)
            {
                if (child != activeChild)
                {
                    child.ResetNode();
                }
            }
        }
    }
}