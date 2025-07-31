using System;
using UnityEngine;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Variables;
using Descent.Common.Attributes.AI;

namespace Descent.AI.BehaviourTree.Nodes
{
    [Serializable]
    [NodeInspectorLabel("Set Variable")]
    public class BehaviourTreeSetVariableNode : BehaviourTreeNode
    {
        [SerializeField]
        [ShowInNodeInspector("Variable")]
        private string _variableGuid;

        [SerializeField]
        [ShowInNodeInspector("Value")]
        private SerializationWrapper _valueWrapper;

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
            object val = _valueWrapper.GetValue();

            contextRegistry.Blackboard.Set(_variableGuid, val);
            Status = BehaviourTreeStatus.Success;
            return Status;
        }

        public override void ResetNode()
        {
            Status = BehaviourTreeStatus.Running;
        }

        public override BehaviourTreeNode CloneNode()
        {
            BehaviourTreeSetVariableNode clone = ScriptableObject.CreateInstance<BehaviourTreeSetVariableNode>();
            clone.Name = Name;
            clone.Position = Position;
            clone._variableGuid = _variableGuid;
            clone._valueWrapper = _valueWrapper;
            return clone;
        }
    }
}