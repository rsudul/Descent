using Descent.AI.BehaviourTree.Variables;

namespace Descent.AI.BehaviourTree.Core
{
    [System.Serializable]
    public class ValuePinDefinition
    {
        public string Name;
        public VariableType Type;
        public PinDirection Direction;

        public ValuePinDefinition(string name, VariableType type, PinDirection direction)
        {
            Name = name;
            Type = type;
            Direction = direction;
        }
    }
}