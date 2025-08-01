using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Variables;
using UnityEditor.UIElements;
using System.Text.RegularExpressions;
using Descent.AI.BehaviourTree.Nodes;

namespace Descent.AI.BehaviourTree.Editor
{
    public class BehaviourTreeVariablesOverlay : VisualElement
    {
        private static readonly List<Type> s_allEnums = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("Descent"))
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return new Type[0]; }
                })
                .Where(t => t.IsEnum)
                .ToList();

        private BehaviourTreeAsset _treeAsset;
        private BehaviourTreeGraphView _graphView;
        private ScrollView _scrollView;

        private Position OverlayPosition = Position.Absolute;
        private float MarginTop = 30.0f;
        private float MarginLeft = 10.0f;
        private float OverlayWidth = 360.0f;
        private StyleLength OverlayHeight = new StyleLength(Length.Percent(50));
        private StyleColor BackgroundColor = new StyleColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
        private float Padding = 10.0f;

        public BehaviourTreeVariablesOverlay(BehaviourTreeAsset treeAsset, BehaviourTreeGraphView graphView)
        {
            _treeAsset = treeAsset;
            _graphView = graphView;
            _scrollView = new ScrollView();

            StyleOverlay();
            Add(_scrollView);
        }

        private void StyleOverlay()
        {
            style.position = OverlayPosition;
            style.top = MarginTop;
            style.left = MarginLeft;
            style.width = OverlayWidth;
            style.height = OverlayHeight;
            style.backgroundColor = BackgroundColor;
            style.paddingLeft = Padding;
            style.paddingRight = Padding;
            style.paddingTop = Padding;
        }

        public void Refresh()
        {
            _scrollView.Clear();

            if (DrawEmptyState())
            {
                return;
            }

            DrawTitle();
            DrawHeader();

            foreach (VariableDefinition variableDefinition in _treeAsset.VariableContainer.Variables)
            {
                DrawVariableRow(variableDefinition);
            }

            DrawAddButton();
        }

        private string GetUniqueName(VariableContainer container, string baseName)
        {
            int i = 1;
            string name = baseName;

            while (container.Variables.Any(v => v.Name == name))
            {
                name = baseName + i;
                i++;
            }

            return name;
        }

        public void SetTreeAsset(BehaviourTreeAsset treeAsset)
        {
            if (_treeAsset == treeAsset)
            {
                return;
            }

            _treeAsset = treeAsset;

            Refresh();
        }

        private List<Type> GetAllEnumTypes() => s_allEnums;

        private bool DrawEmptyState()
        {

            if (_treeAsset == null)
            {
                _scrollView.Add(new Label("No Behaviour Tree asset selected.")
                {
                    style =
                    {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        unityTextAlign = TextAnchor.MiddleCenter,
                    }
                });

                return true;
            }

            if (_treeAsset.VariableContainer == null)
            {
                _scrollView.Add(new Label("No VariableContainer found."));
                return true;
            }

            return false;
        }

        private void DrawTitle()
        {
            _scrollView.Add(new Label("Variables")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            });
        }

        private void DrawHeader()
        {
            VisualElement header = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    marginBottom = 4
                }
            };
            header.Add(new Label(" ") { style = { width = 20 } });
            header.Add(new Label("Name") { style = { width = 100 } });
            header.Add(new Label("Type") { style = { width = 80 } });
            header.Add(new Label("Default") { style = { width = 120 } });
            header.Add(new Label(" ") { style = { width = 20 } });
            _scrollView.Add(header);
        }

        private void DrawVariableRow(VariableDefinition variableDefinition)
        {
            VisualElement row = new VisualElement
            {
                style =
                    {
                        flexDirection = FlexDirection.Column,
                        marginBottom = 8
                    }
            };

            row.Add(DrawLine1(variableDefinition));
            row.Add(DrawLine2(variableDefinition));

            _scrollView.Add(row);
        }

        private VisualElement DrawLine1(VariableDefinition variableDefinition)
        {
            VisualElement line = new VisualElement
            {
                style =
                    {
                        flexDirection = FlexDirection.Row
                    }
            };

            line.Add(DrawDragHandle(variableDefinition));
            line.Add(DrawNameField(variableDefinition));
            line.Add(DrawTypeField(variableDefinition));
            line.Add(DrawDefaultField(variableDefinition));
            line.Add(DrawRemoveButton(variableDefinition));

            return line;
        }

        private VisualElement DrawLine2(VariableDefinition variableDefinition)
        {
            VisualElement line = new VisualElement
            {
                style =
                    {
                        flexDirection = FlexDirection.Row,
                        marginLeft = 20
                    }
            };

            line.Add(DrawDescriptionField(variableDefinition));

            return line;
        }

        private VisualElement DrawDragHandle(VariableDefinition variableDefinition)
        {
            Label dragHandle = new Label("☰")
            {
                style =
                    {
                        width = 16,
                        unityTextAlign = TextAnchor.MiddleCenter,
                        marginRight = 4
                    }
            };
            dragHandle.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button != 0)
                {
                    return;
                }

                DragAndDrop.PrepareStartDrag();
                DragAndDrop.SetGenericData("VariableGUID", variableDefinition.GUID);
                DragAndDrop.objectReferences = new UnityEngine.Object[] { };
                DragAndDrop.StartDrag(variableDefinition.Name);
                evt.StopPropagation();
            });

            return dragHandle;
        }

        private VisualElement DrawNameField(VariableDefinition variableDefinition)
        {
            TextField nameField = new TextField
            {
                value = variableDefinition.Name,
                style =
                    {
                        width = 100
                    }
            };
            nameField.RegisterValueChangedCallback(evt =>
            {
                string newName = evt.newValue;
                if (!Regex.IsMatch(newName, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                {
                    EditorUtility.DisplayDialog("Invalid variable name",
                        "Name must start with a letter or underscore and contain only letters, numbers or _", "OK");
                    nameField.SetValueWithoutNotify(variableDefinition.Name);
                    return;
                }

                bool duplicate = _treeAsset.VariableContainer.Variables
                .Any(v => v != variableDefinition && v.Name == newName);
                if (duplicate)
                {
                    EditorUtility.DisplayDialog("Name duplicate",
                        $"Variable with name '{newName}' already exists.", "OK");
                    nameField.SetValueWithoutNotify(variableDefinition.Name);
                    return;
                }

                variableDefinition.Name = evt.newValue;
                EditorUtility.SetDirty(_treeAsset);
            });

            return nameField;
        }

        private VisualElement DrawTypeField(VariableDefinition variableDefinition)
        {
            EnumField typeField = new EnumField(variableDefinition.VariableType)
            {
                style =
                    {
                        width = 80
                    }
            };
            typeField.Init(variableDefinition.VariableType);
            typeField.RegisterValueChangedCallback(evt =>
            {
                variableDefinition.VariableType = (VariableType)evt.newValue;
                EditorUtility.SetDirty(_treeAsset);
                Refresh();
            });

            return typeField;
        }

        private VisualElement DrawRemoveButton(VariableDefinition variableDefinition)
        {
            VariableContainer container = _treeAsset.VariableContainer;

            Button removeButton = new Button(() =>
            {
                if (TryRemoveVariable(variableDefinition))
                {
                    EditorUtility.SetDirty(_treeAsset);
                    Refresh();
                }
            })
            {
                text = "☒",
                style =
                    {
                        width = 20
                    }
            };

            return removeButton;
        }

        private VisualElement DrawDescriptionField(VariableDefinition variableDefinition)
        {
            TextField descField = new TextField("Description")
            {
                value = variableDefinition.Description,
                style = { flexGrow = 1 }
            };
            descField.RegisterValueChangedCallback(e =>
            {
                variableDefinition.Description = e.newValue;
                EditorUtility.SetDirty(_treeAsset);
            });

            return descField;
        }

        private VisualElement DrawAddButton()
        {
            VariableContainer container = _treeAsset.VariableContainer;

            Button addButton = new Button(() =>
            {
                string newName = GetUniqueName(container, "Variable");
                container.AddVariable(new VariableDefinition(newName, VariableType.Int));
                EditorUtility.SetDirty(_treeAsset);
                Refresh();
            })
            { text = "+ Add variable" };
            _scrollView.Add(addButton);

            VisualElement ioRow = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    marginTop = 8
                }
            };

            Button exportButton = new Button(() =>
            {
                string path = EditorUtility.SaveFilePanel("Export Variables to JSON",
                    Application.dataPath,
                    _treeAsset.name + "_Variables.json",
                    "json");
                if (!string.IsNullOrEmpty(path))
                {
                    VariableContainerIO.ExportToJson(_treeAsset.VariableContainer, path);
                    AssetDatabase.Refresh();
                }
            })
            { text = "Export JSON" };

            Button importButton = new Button(() =>
            {
                string path = EditorUtility.OpenFilePanel("Import Variables from JSON",
                    Application.dataPath,
                    "json");
                if (!string.IsNullOrEmpty(path))
                {
                    VariableContainerIO.ImportFromJson(_treeAsset.VariableContainer, path);
                    EditorUtility.SetDirty(_treeAsset);
                    Refresh();
                }
            })
            { text = "Import JSON" };

            ioRow.Add(exportButton);
            ioRow.Add(importButton);

            _scrollView.Add(ioRow);

            return addButton;
        }

        private VisualElement DrawDefaultField(VariableDefinition variableDefinition)
        {
            switch (variableDefinition.VariableType)
            {
                case VariableType.Int:
                    return DrawIntField(variableDefinition);

                case VariableType.Float:
                    return DrawFloatField(variableDefinition);

                case VariableType.Bool:
                    return DrawBoolField(variableDefinition);

                case VariableType.String:
                    return DrawStringField(variableDefinition);

                case VariableType.Vector2:
                    return DrawVector2Field(variableDefinition);

                case VariableType.Vector3:
                    return DrawVector3Field(variableDefinition);

                case VariableType.Color:
                    return DrawColorField(variableDefinition);

                case VariableType.Enum:
                    return DrawEnumField(variableDefinition);

                default:
                    return new Label("-");
            }
        }

        private VisualElement DrawIntField(VariableDefinition variableDefinition)
        {
            int intValue;
            try
            {
                intValue = Convert.ToInt32(variableDefinition.DefaultValue);
            }
            catch
            {
                intValue = 0;
            }

            IntegerField intField = new IntegerField { value = intValue, style = { width = 60 } };
            intField.RegisterValueChangedCallback(e => { variableDefinition.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });

            return intField;
        }

        private VisualElement DrawFloatField(VariableDefinition variableDefinition)
        {
            float floatValue;
            try
            {
                floatValue = Convert.ToSingle(variableDefinition.DefaultValue);
            }
            catch
            {
                floatValue = 0.0f;
            }

            FloatField floatField = new FloatField { value = floatValue, style = { width = 60 } };
            floatField.RegisterValueChangedCallback(e => { variableDefinition.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });

            return floatField;
        }

        private VisualElement DrawBoolField(VariableDefinition variableDefinition)
        {
            bool boolValue;
            try
            {
                boolValue = Convert.ToBoolean(variableDefinition.DefaultValue);
            }
            catch
            {
                boolValue = false;
            }

            Toggle boolField = new Toggle { value = boolValue, style = { width = 20 } };
            boolField.RegisterValueChangedCallback(e => { variableDefinition.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });

            return boolField;
        }

        private VisualElement DrawStringField(VariableDefinition variableDefinition)
        {
            string strValue = variableDefinition.DefaultValue as string ?? variableDefinition.DefaultValue?.ToString() ?? "";

            TextField textField = new TextField { value = strValue, style = { width = 120 } };
            textField.RegisterValueChangedCallback(e => { variableDefinition.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });

            return textField;
        }

        private VisualElement DrawVector2Field(VariableDefinition variableDefinition)
        {
            Vector2 v2 = variableDefinition.DefaultValue is Vector2 tmpVector2 ? tmpVector2 : Vector2.zero;

            Vector2Field vector2Field = new Vector2Field { value = v2, style = { width = 120 } };
            vector2Field.RegisterValueChangedCallback(e => { variableDefinition.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });

            return vector2Field;
        }

        private VisualElement DrawVector3Field(VariableDefinition variableDefinition)
        {
            Vector3 v3 = variableDefinition.DefaultValue is Vector3 tmpVector3 ? tmpVector3 : Vector3.zero;

            Vector3Field vector3Field = new Vector3Field { value = v3, style = { width = 120 } };
            vector3Field.RegisterValueChangedCallback(e => { variableDefinition.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });

            return vector3Field;
        }

        private VisualElement DrawColorField(VariableDefinition variableDefinition)
        {
            Color c = variableDefinition.DefaultValue is Color tmpColor ? tmpColor : Color.white;

            ColorField colorField = new ColorField { value = c };
            colorField.RegisterValueChangedCallback(e => { variableDefinition.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });

            return colorField;
        }

        private VisualElement DrawEnumField(VariableDefinition variableDefinition)
        {
            Type enumType = Type.GetType(variableDefinition.EnumTypeName);
            List<Type> allEnums = GetAllEnumTypes();

            if (enumType == null)
            {
                List<string> typeNames = allEnums.Select(t => t.AssemblyQualifiedName).ToList();
                List<string> displayNames = allEnums.Select(t => t.Name).ToList();
                int currentIndex = Mathf.Max(0, typeNames.IndexOf(variableDefinition.EnumTypeName));

                PopupField<string> popup = new PopupField<string>("Enum Type", displayNames, currentIndex)
                {
                    style =
                                {
                                    width = 150
                                }
                };
                popup.RegisterValueChangedCallback(evt =>
                {
                    Type selectedClr = allEnums[popup.index];
                    variableDefinition.EnumTypeName = selectedClr.AssemblyQualifiedName;
                    object firstVal = Enum.GetValues(selectedClr).GetValue(0);
                    variableDefinition.DefaultValue = firstVal;
                    EditorUtility.SetDirty(_treeAsset);
                    Refresh();
                });

                return popup;
            }
            Enum current = variableDefinition.DefaultValue is Enum e && e.GetType() == enumType ? e : (Enum)Enum.GetValues(enumType).GetValue(0);

            EnumField enumField = new EnumField(current);
            enumField.Init(current);
            enumField.RegisterValueChangedCallback(evt =>
            {
                variableDefinition.DefaultValue = evt.newValue;
                EditorUtility.SetDirty(_treeAsset);
            });

            return enumField;
        }

        private bool TryRemoveVariable(VariableDefinition variableDefinition)
        {
            var usages = _graphView.NodeViews
                .Where(kvp =>
                {
                    if (kvp.Key is BehaviourTreeGetVariableNode getNode && getNode.VariableGUID == variableDefinition.GUID)
                    {
                        return true;
                    }

                    if (kvp.Key is BehaviourTreeSetVariableNode setNode && setNode.VariableGUID == variableDefinition.GUID)
                    {
                        return true;
                    }

                    return false;
                }).
                ToList();

            if (usages.Count > 0)
            {
                string msg = $"Variable '{variableDefinition.Name}' is used in {usages.Count} nodes.\n" +
                    "Removing this variable will leave those nodes without values.\n" +
                    "Are you sure you want to remove it?";

                if (!EditorUtility.DisplayDialog("Remove variable", msg, "Remove", "Cancel"))
                {
                    return false;
                }
            }

            _treeAsset.VariableContainer.RemoveVariable(variableDefinition.GUID);

            return true;
        }
    }
}