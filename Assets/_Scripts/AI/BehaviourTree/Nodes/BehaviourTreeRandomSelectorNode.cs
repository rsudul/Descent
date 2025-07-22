using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.Common.Attributes.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.AI.BehaviourTree.Nodes
{
    /// <summary>
    /// ---Random Selector---
    /// 
    /// A node which randomly chooses one of its children to execute.
    /// </summary>
    [System.Serializable]
    [NodeInspectorLabel("Random Selector")]
    public class BehaviourTreeRandomSelectorNode : BehaviourTreeCompositeNode
    {
        private int _currentChildIndex = -1;

        [Header("Distribution")]
        [SerializeField, Tooltip("If true: uniform probability. Otherwise use weight list.")]
        [ShowInNodeInspector("Uniform")]
        private bool _uniform = true;
        [SerializeField, Tooltip("Weights for its children. Used when uniform = false.")]
        [ShowInNodeInspector("Weights")]
        private List<float> _weights = new List<float>();

        [Header("Sampling")]
        [SerializeField, Tooltip("If true: a child is chosen randomly every frame. Otherwise a child is chosen once on activation.")]
        [ShowInNodeInspector("Resample each tick")]
        private bool _resampleEachTick = false;

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            if (Children == null || Children.Count == 0)
            {
                Status = BehaviourTreeStatus.Failure;
                return Status;
            }

            if (_currentChildIndex < 0 || _resampleEachTick)
            {
                _currentChildIndex = ChooseChildIndex();
            }

            BehaviourTreeNode child = Children[_currentChildIndex];
            BehaviourTreeStatus childStatus = child.Tick(contextRegistry);

            if (childStatus != BehaviourTreeStatus.Running)
            {
                child.ResetNode();
                _currentChildIndex = -1;
            }

            Status = childStatus;
            return Status;
        }

        public override void ResetNode()
        {
            if (Children != null)
            {
                foreach (BehaviourTreeNode child in Children)
                {
                    child.ResetNode();
                }
            }
            _currentChildIndex = -1;
            Status = BehaviourTreeStatus.Running;
        }

        private int ChooseChildIndex()
        {
            int count = Children.Count;

            if (_uniform)
            {
                return Random.Range(0, count);
            }

            if (_weights == null || _weights.Count != count)
            {
                return Random.Range(0, count);
            }

            float total = 0.0f;
            foreach (float weight in _weights)
            {
                total += Mathf.Max(0.0f, weight);
            }
            if (total <= 0.0f)
            {
                return Random.Range(0, count);
            }

            float roll = Random.value * total;
            float accum = 0.0f;
            for (int i = 0; i < count; i++)
            {
                accum += Mathf.Max(0.0f, _weights[i]);
                if (roll < accum)
                {
                    return i;
                }
            }

            return count - 1;
        }
    }
}