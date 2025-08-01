using UnityEngine;
using Descent.AI.BehaviourTree.Nodes;
using Descent.AI.BehaviourTree.Variables;
using System.Collections.Generic;

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

        public VariableContainer VariableContainer => _variableContainer;

        public IReadOnlyList<ValueConnection> ValueConnections => _valueConnections;

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
        }

        private void OnValidate()
        {
            if (_variableContainer == null)
            {
                _variableContainer = new VariableContainer();
            }
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
    }
}