using Descent.Common.AI.BehaviourTree.Nodes;
using Descent.Common.AI.BehaviourTree.Core;
using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine;
using System.Reflection;
using Descent.Attributes.AI;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    public class BehaviourTreeNodeWithConditionInspectorOverlay : BehaviourTreeNodeInspectorOverlay
    {
        private Type[] _availableConditions;
        private List<string> _availableConditionNames;

        private int? _pendingConditionSelection = null;

        private void DrawConditionField(VisualElement root, SerializedProperty conditionProperty,
            BehaviourTreeNode node, SerializedObject serializedObject)
        {
            if (_availableConditions == null || _availableConditionNames == null)
            {
                CacheConditionTypes();
            }

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
                for (int i = 0; i < _availableConditions.Length; i++)
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
                _customInspector.Clear();
                UpdateSelection(this, node);
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

        public override void UpdateSelection(object sender, UnityEngine.Object obj)
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
                FieldInfo field = pair.Field;
                ShowInNodeInspectorAttribute attr = pair.Attr;
                string label = attr.Label ?? ObjectNames.NicifyVariableName(field.Name);

                if (typeof(IBehaviourTreeCondition).IsAssignableFrom(field.FieldType))
                {
                    SerializedProperty condProp = serializedObject.FindProperty(field.Name);
                    if (condProp != null)
                    {
                        DrawConditionField(_customInspector, condProp, node, serializedObject);
                    }
                    continue;
                }

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
    }
}