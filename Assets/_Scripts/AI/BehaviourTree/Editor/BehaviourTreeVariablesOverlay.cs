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

namespace Descent.AI.BehaviourTree.Editor
{
    public class BehaviourTreeVariablesOverlay : VisualElement
    {
        private BehaviourTreeAsset _treeAsset;
        private ScrollView _scrollView;

        public BehaviourTreeVariablesOverlay(BehaviourTreeAsset treeAsset)
        {
            _treeAsset = treeAsset;
            _scrollView = new ScrollView();

            StyleOverlay();
            Add(_scrollView);
        }

        private void StyleOverlay()
        {
            style.position = Position.Absolute;
            style.top = 30;
            style.left = 10;
            style.width = 360;
            style.height = new StyleLength(Length.Percent(50));
            style.backgroundColor = new StyleColor(new Color(0.0f, 0.0f, 0.0f, 0.6f));
            style.paddingLeft = 10;
            style.paddingRight = 10;
            style.paddingTop = 10;
        }

        public void Refresh()
        {
            _scrollView.Clear();

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

                return;
            }

            VariableContainer container = _treeAsset.VariableContainer;
            if (container == null)
            {
                _scrollView.Add(new Label("No VariableContainer found."));
                return;
            }

            _scrollView.Add(new Label("Variables")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            });

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

            foreach (VariableDefinition def in container.Variables)
            {
                VisualElement row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column,
                        marginBottom = 8
                    }
                };

                VisualElement line1 = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row
                    }
                };

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
                    DragAndDrop.SetGenericData("VariableGUID", def.GUID);
                    DragAndDrop.objectReferences = new UnityEngine.Object[] { };
                    DragAndDrop.StartDrag(def.Name);
                    evt.StopPropagation();
                });
                line1.Add(dragHandle);

                TextField nameField = new TextField
                {
                    value = def.Name,
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
                        nameField.SetValueWithoutNotify(def.Name);
                        return;
                    }
                    
                    bool duplicate = _treeAsset.VariableContainer.Variables
                    .Any(v => v != def && v.Name == newName);
                    if (duplicate)
                    {
                        EditorUtility.DisplayDialog("Name duplicate",
                            $"Variable with name '{newName}' already exists.", "OK");
                        nameField.SetValueWithoutNotify(def.Name);
                        return;
                    }

                    def.Name = evt.newValue;
                    EditorUtility.SetDirty(_treeAsset);
                });
                line1.Add(nameField);

                EnumField typeEnum = new EnumField(def.VariableType)
                {
                    style =
                    {
                        width = 80
                    }
                };
                typeEnum.Init(def.VariableType);
                typeEnum.RegisterValueChangedCallback(evt =>
                {
                    def.VariableType = (VariableType)evt.newValue;
                    EditorUtility.SetDirty(_treeAsset);
                    Refresh();
                });
                line1.Add(typeEnum);

                VisualElement defaultField = new VisualElement();
                switch (def.VariableType)
                {
                    case VariableType.Int:
                        int intValue;
                        try
                        {
                            intValue = Convert.ToInt32(def.DefaultValue);
                        }
                        catch
                        {
                            intValue = 0;
                        }
                        defaultField = new IntegerField { value = intValue, style = { width = 60 } };
                        ((IntegerField)defaultField).RegisterValueChangedCallback(e => { def.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });
                        break;

                    case VariableType.Float:
                        float floatValue;
                        try
                        {
                            floatValue = Convert.ToSingle(def.DefaultValue);
                        }
                        catch
                        {
                            floatValue = 0.0f;
                        }
                        defaultField = new FloatField { value = floatValue, style = { width = 60 } };
                        ((FloatField)defaultField).RegisterValueChangedCallback(e => { def.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });
                        break;

                    case VariableType.Bool:
                        bool boolValue;
                        try
                        {
                            boolValue = Convert.ToBoolean(def.DefaultValue);
                        }
                        catch
                        {
                            boolValue = false;
                        }
                        defaultField = new Toggle { value = boolValue, style = { width = 20 } };
                        ((Toggle)defaultField).RegisterValueChangedCallback(e => { def.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });
                        break;

                    case VariableType.String:
                        string strValue = def.DefaultValue as string ?? def.DefaultValue?.ToString() ?? "";
                        defaultField = new TextField { value = strValue, style = { width = 120 } };
                        ((TextField)defaultField).RegisterValueChangedCallback(e => { def.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });
                        break;

                    case VariableType.Vector2:
                        Vector2 v2 = def.DefaultValue is Vector2 tmpVector2 ? tmpVector2 : Vector2.zero;
                        defaultField = new Vector2Field { value = v2, style = { width = 120 } };
                        ((Vector2Field)defaultField).RegisterValueChangedCallback(e => { def.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });
                        break;

                    case VariableType.Vector3:
                        Vector3 v3 = def.DefaultValue is Vector3 tmpVector3 ? tmpVector3 : Vector3.zero;
                        defaultField = new Vector3Field { value = v3, style = { width = 120 } };
                        ((Vector3Field)defaultField).RegisterValueChangedCallback(e => { def.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });
                        break;

                    case VariableType.Color:
                        Color c = def.DefaultValue is Color tmpColor ? tmpColor : Color.white;
                        defaultField = new ColorField { value = c };
                        ((ColorField)defaultField).RegisterValueChangedCallback(e => { def.DefaultValue = e.newValue; EditorUtility.SetDirty(_treeAsset); });
                        break;

                    case VariableType.Enum:
                        Type enumType = Type.GetType(def.EnumTypeName);
                        List<Type> allEnums = GetAllEnumTypes();

                        if (enumType == null)
                        {
                            List<string> typeNames = allEnums.Select(t => t.AssemblyQualifiedName).ToList();
                            List<string> displayNames = allEnums.Select(t => t.Name).ToList();
                            int currentIndex = Mathf.Max(0, typeNames.IndexOf(def.EnumTypeName));

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
                                def.EnumTypeName = selectedClr.AssemblyQualifiedName;
                                object firstVal = Enum.GetValues(selectedClr).GetValue(0);
                                def.DefaultValue = firstVal;
                                EditorUtility.SetDirty(_treeAsset);
                                Refresh();
                            });
                            defaultField = popup;
                        }
                        else
                        {
                            Enum current = def.DefaultValue is Enum e && e.GetType() == enumType ? e : (Enum)Enum.GetValues(enumType).GetValue(0);

                            EnumField enumField = new EnumField(current);
                            enumField.Init(current);
                            enumField.RegisterValueChangedCallback(evt =>
                            {
                                def.DefaultValue = evt.newValue;
                                EditorUtility.SetDirty(_treeAsset);
                            });
                            defaultField = enumField;
                        }

                        break;

                    default:
                        defaultField = new Label("-");
                        break;
                }
                line1.Add(defaultField);

                Button removeButton = new Button(() =>
                {
                    container.RemoveVariable(def.GUID);
                    EditorUtility.SetDirty(_treeAsset);
                    Refresh();
                })
                {
                    text = "☒",
                    style =
                    {
                        width = 20
                    }
                };
                line1.Add(removeButton);

                row.Add(line1);

                VisualElement line2 = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        marginLeft = 20
                    }
                };

                /*if (def.VariableType == VariableType.Int ||  def.VariableType == VariableType.Float)
                {
                    FloatField minField = new FloatField("Min")
                    {
                        value = def.MinValue ?? 0.0f,
                        style = { width = 60 }
                    };
                    minField.RegisterValueChangedCallback(e =>
                    {
                        def.MinValue = e.newValue;
                        EditorUtility.SetDirty(_treeAsset);
                    });
                    line2.Add(minField);

                    FloatField maxField = new FloatField("Max")
                    {
                        value = def.MaxValue ?? 0.0f,
                        style = { width = 60 }
                    };
                    maxField.RegisterValueChangedCallback(e =>
                    {
                        def.MaxValue = e.newValue;
                        EditorUtility.SetDirty(_treeAsset);
                    });
                    line2.Add(maxField);
                }*/

                TextField descField = new TextField("Description")
                {
                    value = def.Description,
                    style = { flexGrow = 1 }
                };
                descField.RegisterValueChangedCallback(e =>
                {
                    def.Description = e.newValue;
                    EditorUtility.SetDirty(_treeAsset);
                });
                line2.Add(descField);

                row.Add(line2);

                _scrollView.Add(row);
            }

            Button addButton = new Button(() =>
            {
                string newName = GetUniqueName(container, "Variable");
                container.AddVariable(new VariableDefinition(newName, VariableType.Int));
                EditorUtility.SetDirty(_treeAsset);
                Refresh();
            }) { text = "+ Add variable" };
            _scrollView.Add(addButton);
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

        private List<Type> GetAllEnumTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("Descent"))
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return new Type[0]; }
                })
                .Where(t => t.IsEnum)
                .ToList();
        }
    }
}