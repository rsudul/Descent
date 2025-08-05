using System.Collections.Generic;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Requests;
using Descent.AI.BehaviourTree.Variables;

namespace Descent.AI.BehaviourTree.Actions
{
    [System.Serializable]
    public abstract class BehaviourTreeActionWithPins : IBehaviourTreeActionWithPins
    {
        protected string NodeGuid { get; set; }

        protected T GetPinValue<T>(string pinName, BehaviourTreeContextRegistry contextRegistry)
        {
            return contextRegistry.GetPinValue<T>(NodeGuid, pinName);
        }

        protected bool HasPinConnection(string pinName, BehaviourTreeContextRegistry contextRegistry)
        {
            return contextRegistry.HasPinConnection(NodeGuid, pinName);
        }

        public void SetNodeGuid(string guid) => NodeGuid = guid;

        protected ValuePinDefinition InputPin(string name, VariableType type) =>
            new ValuePinDefinition(name, type, PinDirection.Input);

        public abstract BehaviourTreeStatus Execute(BehaviourTreeContextRegistry contextRegistry);
        public abstract IEnumerable<ValuePinDefinition> GetRequiredPins();
        public abstract IBehaviourTreeAction Clone();
        public virtual void ResetAction() { }
        public virtual void InjectDispatcher(BehaviourTreeActionRequestDispatcher dispatcher) { }
    }
}