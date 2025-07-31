using System;
using UnityEngine;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Variables;
using Descent.Common.Attributes.AI;

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
            clone._cachedValue = _cachedValue;
            return clone;
        }
    }
}