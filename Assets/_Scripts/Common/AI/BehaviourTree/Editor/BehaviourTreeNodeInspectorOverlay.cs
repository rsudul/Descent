using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.Common.AI.BehaviourTree.Nodes;
using UnityEditor.UIElements;

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

            Label typeLabel = new Label(node.GetType().Name)
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

            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);

            while (property.NextVisible(false))
            {
                if (property.name is "m_Script" or "Parent" or "Children")
                {
                    continue;
                }

                PropertyField field = new PropertyField(property, ObjectNames.NicifyVariableName(property.name));
                field.Bind(serializedObject);
                _customInspector.Add(field);
            }
        }

        public virtual void ClearInspector()
        {
            _customInspector.Clear();
        }
    }
}