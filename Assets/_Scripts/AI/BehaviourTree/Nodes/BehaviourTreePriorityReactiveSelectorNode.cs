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
        [SerializeField]
        private int _activeIndex = -1;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children?.Count == 0)
            {
                return Status = BehaviourTreeStatus.Failure;
            }

            int candidate = -1;
            for (int i=0; i<Children.Count; i++)
            {
                if (CanRunChild(Children[i], contextRegistry))
                {
                    candidate = i;
                    break;
                }
            }

            if (candidate != _activeIndex)
            {
                if (_activeIndex >= 0 && _activeIndex < Children.Count)
                {
                    Children[_activeIndex].ResetNode();
                }
                _activeIndex = candidate;
            }

            if (_activeIndex < 0)
            {
                return Status = BehaviourTreeStatus.Failure;
            }

            BehaviourTreeStatus childStatus = Children[_activeIndex].Tick(contextRegistry);

            return Status = childStatus;
        }

        public override void ResetNode()
        {
            if (_activeIndex >= 0 && _activeIndex < Children.Count)
            {
                Children[_activeIndex].ResetNode();
            }
            _activeIndex = -1;
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

        private bool CanRunChild(BehaviourTreeNode node, BehaviourTreeContextRegistry contextRegistry)
        {
            if (node is BehaviourTreeGuardNode guard)
            {
                return guard.Condition == null || guard.Condition.Check(contextRegistry);
            }

            return true;
        }
    }
}