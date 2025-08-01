using UnityEngine;
using Descent.AI.BehaviourTree.Nodes;
using Descent.AI.BehaviourTree.Variables;
using System.Collections.Generic;
using UnityEditor;

namespace Descent.AI.BehaviourTree.Core
{
    [CreateAssetMenu(menuName = "Descent/AI/BehaviourTree")]
    public class BehaviourTreeAsset : ScriptableObject
    {
        [SerializeField]
        public BehaviourTreeNode Root;

        [SerializeField]
        private VariableContainer _variableContainer = new VariableContainer();

        [SerializeField]
        private List<ValueConnection> _valueConnections = new List<ValueConnection>();

        [SerializeField]
        private List<BehaviourTreeNode> _allNodes = new List<BehaviourTreeNode>();

        public VariableContainer VariableContainer => _variableContainer;

        public IReadOnlyList<ValueConnection> ValueConnections => _valueConnections;

        public IReadOnlyList<BehaviourTreeNode> AllNodes => _allNodes;

        public BehaviourTreeNode CloneTree()
        {
            return Root.CloneNode();
        }

        private void OnEnable()
        {
            if (_variableContainer == null)
            {
                _variableContainer = new VariableContainer();
            }

#if UNITY_EDITOR
            SyncAllNodes();
#endif
        }

        private void OnValidate()
        {
            if (_variableContainer == null)
            {
                _variableContainer = new VariableContainer();
            }

#if UNITY_EDITOR
            SyncAllNodes();
#endif
        }

        public void AddValueConnection(ValueConnection valueConnection)
        {
            _valueConnections.Add(valueConnection);
        }

        public void RemoveValueConnection(ValueConnection valueConnection)
        {
            _valueConnections.RemoveAll(c =>
            c.SourceNodeGUID == valueConnection.SourceNodeGUID &&
            c.SourcePinName == valueConnection.SourcePinName &&
            c.TargetNodeGUID == valueConnection.TargetNodeGUID &&
            c.TargetPinName == valueConnection.TargetPinName);
        }

        private void SyncAllNodes()
        {
#if UNITY_EDITOR
            _allNodes.Clear();
            if (Root != null)
            {
                CollectRecursive(Root);
            }
            EditorUtility.SetDirty(this);
#endif
        }

        private void CollectRecursive(BehaviourTreeNode node)
        {
#if UNITY_EDITOR
            _allNodes.Add(node);

            if (node is BehaviourTreeCompositeNode compositeNode)
            {
                foreach (BehaviourTreeNode child in compositeNode.Children)
                {
                    CollectRecursive(child);
                }
            }
            else if (node is BehaviourTreeRepeatUntilFailureNode repeatUntilFailureNode)
            {
                if (repeatUntilFailureNode.Child != null)
                {
                    CollectRecursive(repeatUntilFailureNode.Child);
                }
            }
#endif
        }
    }
}