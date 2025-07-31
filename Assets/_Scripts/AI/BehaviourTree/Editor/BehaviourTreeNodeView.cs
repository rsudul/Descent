using UnityEditor.Experimental.GraphView;
using Descent.AI.BehaviourTree.Nodes;
using UnityEngine.UIElements;
using UnityEngine;
using Descent.AI.BehaviourTree.Core;
using UnityEditor;
using Descent.AI.BehaviourTree.Variables;

namespace Descent.AI.BehaviourTree.Editor
{
    public class BehaviourTreeNodeView : Node
    {
        public BehaviourTreeNode Node { get; private set; }
        public Port Input { get; private set; }
        public Port Output { get; private set; }

        public BehaviourTreeNodeView(BehaviourTreeNode node)
        {
            Node = node;
            title = Node.Name ?? Node.GetType().Name;

            Node.OnPropertyChanged += OnNodePropertyChanged;

            SetStyles(node);

            if (node is not BehaviourTreeGetVariableNode)
            {
                Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                Input.portName = "";
                inputContainer.Add(Input);
            }

            if (node is BehaviourTreeCompositeNode)
            {
                Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
                Output.portName = "";
                outputContainer.Add(Output);
            }

            AddValuePins();

            RegisterCallback<GeometryChangedEvent>(OnMoved);

            RefreshExpandedState();
            RefreshPorts();
        }

        public void CleanUp()
        {
            Node.OnPropertyChanged -= OnNodePropertyChanged;
        }

        private void SetStyles(BehaviourTreeNode node)
        {
            AddToClassList("node");

            if (node is BehaviourTreeSelectorNode)
            {
                AddToClassList("selector");
            }
            else if (node is BehaviourTreeSequenceNode)
            {
                AddToClassList("sequence");
            }
            else if (node is BehaviourTreeActionNode)
            {
                AddToClassList("action");
            }
            else if (node is BehaviourTreeConditionNode)
            {
                AddToClassList("condition");
            }
        }

        public void UpdateTitle()
        {
            title = Node.Name ?? Node.GetType().Name;
            MarkDirtyRepaint();
        }

        private void OnMoved(GeometryChangedEvent evt)
        {
            Vector2 newPos = GetPosition().position;
            if (Node.Position != newPos)
            {
                Node.Position = newPos;
                EditorUtility.SetDirty(Node);
                AssetDatabase.SaveAssets();
            }
        }

        public void SetStatus(BehaviourTreeStatus status)
        {
            switch (status)
            {
                case BehaviourTreeStatus.Running:
                    titleContainer.style.backgroundColor = new Color(0.4f, 0.6f, 1.0f, 0.9f);
                    break;

                case BehaviourTreeStatus.Success:
                    titleContainer.style.backgroundColor = new Color(0.4f, 1.0f, 0.4f, 0.8f);
                    break;

                case BehaviourTreeStatus.Failure:
                    titleContainer.style.backgroundColor = new Color(1.0f, 0.4f, 0.4f, 0.8f);
                    break;

                default:
                    titleContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.2f);
                    break;
            }
        }

        private void OnNodePropertyChanged(object sender, string propertyName)
        {
            if (propertyName == nameof(BehaviourTreeNode.Name))
            {
                UpdateTitle();
            }

            EditorUtility.SetDirty(Node);
            AssetDatabase.SaveAssets();
        }

        private void AddValuePins()
        {
            foreach (var valuePin in Node.ValuePins)
            {
                Port port = InstantiatePort(Orientation.Horizontal,
                    valuePin.Direction == PinDirection.Input ? Direction.Input : Direction.Output,
                    Port.Capacity.Single,
                    GetPortDataType(valuePin.Type));
                port.portName = valuePin.Name;
                port.userData = valuePin;

                port.style.alignSelf = Align.Center;
                if (valuePin.Direction == PinDirection.Output)
                {
                    port.style.marginLeft = new StyleLength(StyleKeyword.Auto);
                }
                port.portColor = GetColorForType(valuePin.Type);

                if (valuePin.Direction == PinDirection.Input)
                {
                    inputContainer.Add(port);
                }
                else
                {
                    outputContainer.Add(port);
                }
            }
        }

        private static System.Type GetPortDataType(VariableType variableType)
        {
            switch (variableType)
            {
                case VariableType.Int: return typeof(int);
                case VariableType.Float: return typeof(float);
                case VariableType.Bool: return typeof(bool);
                case VariableType.String: return typeof(string);
                case VariableType.Vector2: return typeof(Vector2);
                case VariableType.Vector3: return typeof(Vector3);
                case VariableType.Color: return typeof(Color);
                default: return typeof(object);
            }
        }
        private static Color GetColorForType(VariableType type)
        {
            switch (type)
            {
                case VariableType.Int:
                case VariableType.Float:    return Color.green;
                case VariableType.Bool:     return Color.yellow;
                case VariableType.String:   return Color.cyan;
                case VariableType.Vector2:
                case VariableType.Vector3:  return Color.blue;
                case VariableType.Color:    return Color.magenta;
                default:                    return Color.gray;
            }
        }
    }
}