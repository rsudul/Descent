using System.Collections.Generic;
using Descent.AI.BehaviourTree.Context;
using Descent.AI.BehaviourTree.Conditions;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Variables;

namespace Descent.AI.BehaviourTree.Conditions
{
    [System.Serializable]
    public abstract class BehaviourTreeConditionWithPins : IBehaviourTreeConditionWithPins
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

        public abstract bool Check(BehaviourTreeContextRegistry contextRegistry);
        public abstract IEnumerable<ValuePinDefinition> GetRequiredPins();
        public abstract IBehaviourTreeCondition Clone();
        public virtual void ResetCondition() { }
    }
}