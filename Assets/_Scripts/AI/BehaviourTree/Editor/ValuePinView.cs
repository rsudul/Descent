using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Variables;

namespace Descent.AI.BehaviourTree.Editor
{
    public class ValuePinView : Port
    {
        public ValuePinDefinition Definition { get; private set; }

        public ValuePinView(ValuePinDefinition definition) : base(Orientation.Horizontal,
            definition.Direction == PinDirection.Input ? Direction.Input : Direction.Output,
            Capacity.Single,
            GetPortDataType(definition.Type))
        {
            Definition = definition;
            portName = Definition.Name;
            style.flexDirection = FlexDirection.Row;
            style.flexShrink = 0;
            style.alignSelf = Align.Center;
            if (Definition.Direction == PinDirection.Output)
            {
                style.marginLeft = new StyleLength(StyleKeyword.Auto);
            }
            pickingMode = PickingMode.Position;
            SetupColor(Definition.Type);
        }

        private void SetupColor(VariableType type)
        {
            Color c = Color.black;

            switch (type)
            {
                case VariableType.Int:
                case VariableType.Float:    c = Color.green; break;
                case VariableType.Bool:     c = Color.yellow; break;
                case VariableType.String:   c = Color.cyan; break;
                case VariableType.Vector2:
                case VariableType.Vector3:  c = Color.blue; break;
                case VariableType.Color:    c = Color.magenta; break;
                default:                    c = Color.gray; break;
            }

            portColor = c;
        }

        private static System.Type GetPortDataType(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.Int:      return typeof(int);
                case VariableType.Float:    return typeof(float);
                case VariableType.Bool:     return typeof(bool);
                case VariableType.String:   return typeof(string);
                case VariableType.Vector2:  return typeof(Vector2);
                case VariableType.Vector3:  return typeof(Vector3);
                case VariableType.Color:    return typeof(Color);
                default:                    return typeof(object);
            }
        }
    }
}