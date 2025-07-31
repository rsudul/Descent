using System;
using UnityEngine;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Variables;
using Descent.Common.Attributes.AI;
using System.Collections.Generic;

namespace Descent.AI.BehaviourTree.Nodes
{
    [Serializable]
    [NodeInspectorLabel("Get Variable")]
    public class BehaviourTreeGetVariableNode : BehaviourTreeNode
    {
        [SerializeField]
        [ShowInNodeInspector("Variable")]
        private string _variableGuid;
        [SerializeField]
        [ShowInNodeInspector("Variable Type")]
        private VariableType _variableType;

        [SerializeField]
        [ShowInNodeInspector("Value")]
        private SerializationWrapper _cachedValue;

        public string VariableGUID
        {
            get => _variableGuid;
            set
            {
                _variableGuid = value;
            }
        }

        public VariableType VariableType
        {
            get => _variableType;
            set
            {
                _variableType = value;
            }
        }

        public override IEnumerable<ValuePinDefinition> ValuePins
        {
            get
            {
                yield return new ValuePinDefinition("", _variableType, PinDirection.Output);
            }
        }

        public override BehaviourTreeStatus Tick(BehaviourTreeContextRegistry contextRegistry)
        {
            object val = contextRegistry.Blackboard.Get<object>(_variableGuid, null);
            _cachedValue = new SerializationWrapper(val);
            Status = BehaviourTreeStatus.Success;
            return Status;
        }

        public override void ResetNode()
        {
            Status = BehaviourTreeStatus.Running;
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeGetVariableNode clone = ScriptableObject.CreateInstance<BehaviourTreeGetVariableNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone._variableGuid = _variableGuid;
            clone._variableType = _variableType;
            clone._cachedValue = _cachedValue;
            return clone;
        }
    }
}