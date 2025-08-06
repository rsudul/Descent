using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Nodes;
using Descent.AI.BehaviourTree.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Descent.AI.BehaviourTree.Context
{
    public class BehaviourTreeContextRegistry
    {
        private readonly Dictionary<Type, BehaviourTreeContext> _contexts = new Dictionary<Type, BehaviourTreeContext>();

        private Dictionary<(string nodeGuid, string pinName), ValueConnection> _valueLookup;

        private Dictionary<string, BehaviourTreeNode> _nodeInstances = new Dictionary<string, BehaviourTreeNode>();

        public int Count => _contexts.Count;

        public Blackboard Blackboard { get; } = new Blackboard();

        public BehaviourTreeContextRegistry(VariableContainer container, IEnumerable<ValueConnection> valueConnections)
        {
            foreach (VariableDefinition def in container.Variables)
            {
                Blackboard.Set(def.GUID, def.DefaultValue);
            }

            _valueLookup = valueConnections.ToDictionary(vc => (vc.TargetNodeGUID, vc.TargetPinName), vc => vc);
        }

        public void RegisterContext(Type contextType, BehaviourTreeContext context)
        {
            _contexts[contextType] = context;
        }

        public BehaviourTreeContext GetContext(Type contextType)
        {
            if (!_contexts.TryGetValue(contextType, out BehaviourTreeContext context))
            {
                return null;
            }

            return context;
        }

        public bool TryGetValueConnection(string targetNodeGuid, string targetPinName, out ValueConnection valueConnection)
        {
            if (_valueLookup != null
                && _valueLookup.TryGetValue((targetNodeGuid, targetPinName), out valueConnection))
            {
                return true;
            }

            valueConnection = null;

            return false;
        }

        public void RegisterNodeInstance(string guid, BehaviourTreeNode node)
        {
            _nodeInstances[guid] = node;
        }

        public BehaviourTreeNode GetNodeInstance(string guid) => _nodeInstances.TryGetValue(guid, out var n) ? n : null;

        public T GetPinValue<T>(string nodeGuid, string pinName)
        {
            if (TryGetValueConnection(nodeGuid, pinName, out ValueConnection valueConnection))
            {
                BehaviourTreeGetVariableNode source = GetNodeInstance(valueConnection.SourceNodeGUID)
                    as BehaviourTreeGetVariableNode;
                if (source != null)
                {
                    source.Tick(this);
                    return (T)source.CachedValue.GetValue();
                }
            }

            return default(T);
        }

        public bool HasPinConnection(string nodeGuid, string pinName)
        {
            return TryGetValueConnection(nodeGuid, pinName, out _);
        }

        public T GetVariableValue<T>(string variableGuid)
        {
            return Blackboard.Get<T>(variableGuid);
        }
    }
}