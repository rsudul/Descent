using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.Common.AI.BehaviourTree.Nodes;
using UnityEditor.UIElements;
using System.Reflection;
using System.Linq;
using Descent.Attributes.AI;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    public class BehaviourTreeNodeInspectorOverlay : VisualElement
    {
        protected VisualElement _customInspector;

        public BehaviourTreeNodeInspectorOverlay()
        {
            StyleOverlay();

            VisualElement headerRow = new VisualElement()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = -4
                }
            };

            Label nameLabel = new Label("Node")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    fontSize = 14
                }
            };
            headerRow.Add(nameLabel);

            VisualElement spacer = new VisualElement()
            {
                style =
                {
                    flexGrow = 1
                }
            };
            headerRow.Add(spacer);

            var closeBtn = new Button(() => visible = false)
            {
                text = "✕",
                style =
                {
                    alignSelf = Align.FlexEnd,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    fontSize = 10,
                    color = new StyleColor(Color.white),
                    marginBottom = 4,
                    marginTop = -6,
                    width = 20,
                    height = 20
                }
            };
            headerRow.Add(closeBtn);

            Add(headerRow);

            _customInspector = new VisualElement();
            _customInspector.style.marginTop = 8;
            Add(_customInspector);

            visible = false;
        }

        public virtual void UpdateSelection(object sender, UnityEngine.Object obj)
        {
            _customInspector.Clear();

            if (obj == null)
            {
                visible = false;
                return;
            }

            visible = true;

            if (obj is not BehaviourTreeNode node)
            {
                _customInspector.Add(new Label("Unsupported node type!"));
                return;
            }

            VisualElement headerRow = ElementAt(0);
            Label nameLabel = headerRow?.ElementAt(0) as Label;
            if (nameLabel != null)
            {
                nameLabel.text = node.Name ?? node.GetType().Name;
            }

            string nodeType = GetNodeType(node);
            Label typeLabel = new Label(nodeType)
            {
                style =
                {
                    color = new StyleColor(new Color(1.0f, 1.0f, 1.0f, 0.5f)),
                    fontSize = 12,
                    marginBottom = 10
                }
            };

            _customInspector.Add(typeLabel);

            SerializedObject serializedObject = new SerializedObject(node);

            var fields = node.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(f => new
                {
                    Field = f,
                    Attr = f.GetCustomAttribute<ShowInNodeInspectorAttribute>()
                })
                .Where(x => x.Attr != null)
                .OrderByDescending(x => x.Attr.Priority);

            FieldInfo nameField = null;

            foreach (var pair in fields)
            {
                var field = pair.Field;
                var attr = pair.Attr;
                string label = attr.Label ?? ObjectNames.NicifyVariableName(field.Name);

                bool isNodeName = field.GetCustomAttribute<NodeNameFieldAttribute>() != null;
                if (isNodeName)
                {
                    nameField = field;
                }

                SerializedProperty property = serializedObject.FindProperty(field.Name);
                if (property == null)
                {
                    continue;
                }

                PropertyField fieldElement = new PropertyField(property, label);

                if (isNodeName)
                {
                    fieldElement.RegisterValueChangeCallback(evt =>
                    {
                        if (node is BehaviourTreeNode btNode)
                        {
                            btNode.Name = evt.changedProperty.stringValue;
                        }
                    });
                }

                fieldElement.Bind(serializedObject);
                _customInspector.Add(fieldElement);
            }
        }

        public virtual void ClearInspector()
        {
            _customInspector.Clear();
        }

        protected string GetNodeType(BehaviourTreeNode node)
        {
            string customName = node.GetType().GetCustomAttribute<NodeInspectorLabelAttribute>()?.Label;
            return customName ?? node.GetType().Name;
        }

        protected virtual void StyleOverlay()
        {
            style.position = Position.Absolute;
            style.top = 0;
            style.right = 0;

            style.maxWidth = 320;
            style.minWidth = 270;

            style.marginTop = 40;
            style.marginRight = 16;
            style.paddingTop = 16;
            style.paddingLeft = 16;
            style.paddingRight = 16;
            style.paddingBottom = 12;

            style.backgroundColor = new StyleColor(new Color(0.0f, 0.0f, 0.0f, 0.25f));

            style.flexDirection = FlexDirection.Column;
        }
    }
}