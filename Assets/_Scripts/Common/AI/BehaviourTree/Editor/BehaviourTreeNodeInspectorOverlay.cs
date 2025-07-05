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
            style.position = Position.Absolute;
            style.width = 360;
            style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0.7f));
            style.borderBottomLeftRadius = 10;
            style.borderBottomRightRadius = 10;
            style.borderTopLeftRadius = 10;
            style.borderTopRightRadius = 10;
            style.paddingLeft = 18;
            style.paddingRight = 18;
            style.paddingTop = 18;
            style.paddingBottom = 12;
            style.marginTop = 16;
            style.marginRight = 16;
            style.flexDirection = FlexDirection.Column;
            style.alignItems = Align.FlexStart;

            var closeBtn = new Button(() => visible = false)
            {
                text = "✕",
                style =
                {
                    alignSelf = Align.FlexEnd,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    fontSize = 12,
                    color = new StyleColor(Color.gray),
                    marginBottom = 4,
                    marginTop = -6
                }
            };
            Add(closeBtn);

            _customInspector = new VisualElement();
            _customInspector.style.marginTop = 12;
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

            Label nameLabel = new Label(node.Name ?? node.GetType().Name)
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    fontSize = 18,
                    marginBottom = 2
                }
            };
            _customInspector.Add(nameLabel);

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

            foreach (var pair in fields)
            {
                var field = pair.Field;
                var attr = pair.Attr;
                string label = attr.Label ?? ObjectNames.NicifyVariableName(field.Name);

                SerializedProperty property = serializedObject.FindProperty(field.Name);
                if (property == null)
                {
                    continue;
                }

                PropertyField fieldElement = new PropertyField(property, label);
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
    }
}