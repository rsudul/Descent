using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.Common.AI.BehaviourTree.Nodes;
using UnityEditor.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;
using Descent.Common.AI.BehaviourTree.Core;

namespace Descent.Common.AI.BehaviourTree.Editor
{

    public class BehaviourTreeNodeInspectorOverlay : VisualElement
    {
        private VisualElement _customInspector;

        private Type[] _availableConditions;
        private List<string> _availableConditionNames;

        private int? _pendingConditionSelection = null;

        public BehaviourTreeNodeInspectorOverlay()
        {
            CacheConditionTypes();

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

        public void UpdateSelection(object sender, UnityEngine.Object obj)
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

            if (_pendingConditionSelection.HasValue)
            {
                SerializedProperty condProp = serializedObject.FindProperty("Condition");
                int index = _pendingConditionSelection.Value;
                if (index == 0)
                {
                    condProp.managedReferenceValue = null;
                } else if (index > 0)
                {
                    condProp.managedReferenceValue = Activator.CreateInstance(_availableConditions[index - 1]);
                }
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(node);
                serializedObject.Update();
                _pendingConditionSelection = null;
            }

            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);

            while (property.NextVisible(false))
            {
                if (property.name is "m_Script" or "Parent" or "Children")
                {
                    continue;
                }

                if (property.name == "Condition")
                {
                    DrawConditionField(_customInspector, property, node, serializedObject);
                    continue;
                }

                PropertyField field = new PropertyField(property, ObjectNames.NicifyVariableName(property.name));
                field.Bind(serializedObject);
                _customInspector.Add(field);
            }
        }

        private void DrawConditionField(VisualElement root, SerializedProperty conditionProperty,
            BehaviourTreeNode node, SerializedObject serializedObject)
        {
            VisualElement conditionRow = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    alignItems = Align.Center,
                    marginBottom = 8
                }
            };

            Label ifLabel = new Label("If")
            {
                style =
                {
                    marginRight = 4
                }
            };
            conditionRow.Add(ifLabel);

            int currentIndex = 0;
            if (conditionProperty.managedReferenceValue != null)
            {
                Type currentType = conditionProperty.managedReferenceValue.GetType();
                for (int i=0; i<_availableConditions.Length; i++)
                {
                    if (_availableConditions[i] == currentType)
                    {
                        currentIndex = i + 1;
                        break;
                    }
                }
            }

            PopupField<string> dropdown = new PopupField<string>(_availableConditionNames, currentIndex)
            {
                style =
                {
                    minWidth = 120,
                    maxWidth = 180,
                    marginRight = 8
                }
            };
            dropdown.RegisterValueChangedCallback(evt =>
            {
                int index = _availableConditionNames.IndexOf(evt.newValue);
                _pendingConditionSelection = index;
                BehaviourTreeGraphWindow window = EditorWindow.GetWindow<BehaviourTreeGraphWindow>();
                window.RefreshNodeInspector(node);
            });
            conditionRow.Add(dropdown);

            Label isLabel = new Label("is")
            {
                style =
                {
                    marginRight = 4
                }
            };
            conditionRow.Add(isLabel);

            if (conditionProperty.managedReferenceValue != null)
            {
                PropertyField conditionField = new PropertyField(conditionProperty, "")
                {
                    style =
                    {
                        minWidth = 100,
                        marginRight = 8
                    }
                };
                conditionRow.Add(conditionField);
            }

            if (node.GetType().Name == "BehaviourTreeRepeatWhileConditionNode")
            {
                SerializedProperty invertProp = serializedObject.FindProperty("_invert");
                if (invertProp != null)
                {
                    PopupField<string> tfDropdown = new PopupField<string>(new List<string> { "TRUE", "FALSE" },
                        invertProp.boolValue ? "FALSE" : "TRUE");
                    tfDropdown.RegisterValueChangedCallback(evt =>
                    {
                        invertProp.boolValue = evt.newValue == "FALSE";
                        serializedObject.ApplyModifiedProperties();
                    });
                    tfDropdown.style.minWidth = 64;
                    conditionRow.Add(tfDropdown);
                }
            }

            root.Add(conditionRow);
        }

        private void CacheConditionTypes()
        {
            _availableConditions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IBehaviourTreeCondition).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .OrderBy(t => t.Name)
                .ToArray();

            _availableConditionNames = new List<string> { "null" };
            _availableConditionNames.AddRange(_availableConditions.Select(t => t.Name));
        }

        public void ClearInspector()
        {
            _customInspector.Clear();
        }
    }
}