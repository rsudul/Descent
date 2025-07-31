using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.AI.BehaviourTree.Core;
using UnityEditor.UIElements;
using System;
using System.Reflection;
using Descent.AI.BehaviourTree.Nodes;
using Descent.Common.Attributes.AI;

namespace Descent.AI.BehaviourTree.Editor
{
    public class BehaviourTreeGraphWindow : EditorWindow
    {
        private BehaviourTreeGraphView _graphView;
        private BehaviourTreeNodeInspectorOverlay _inspectorOverlay;
        private BehaviourTreeVariablesOverlay _variablesOverlay;
        private BehaviourTreeAsset _treeAsset;

        [MenuItem("Window/Descent/AI/Behaviour Tree Graph Editor")]
        public static void OpenWindow()
        {
            BehaviourTreeGraphWindow window = GetWindow<BehaviourTreeGraphWindow>();
            window.titleContent = new GUIContent("Behaviour Tree Graph");
        }

        private void OnInspectorUpdate()
        {
            if (Application.isPlaying && _graphView != null)
            {
                _graphView.RefreshNodeStatuses();
            }
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            InitializeVariablesOverlay();
            InitializeDragAndDrop();
        }

        private void OnDisable()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            Save();

            if (_inspectorOverlay != null)
            {
                _graphView.OnNodeSelected -= _inspectorOverlay.UpdateSelection;
                _graphView.OnNodeDeleted -= OnNodeDeleted;
            }

            rootVisualElement.Remove(_graphView);
        }

        private void ConstructGraphView()
        {
            _graphView = new BehaviourTreeGraphView(this)
            {
                name = "Behaviour Tree Graph"
            };
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);

            _graphView.OnNodeSelected += (sender, node) => RefreshNodeInspector(node);
            _graphView.OnNodeDeleted += OnNodeDeleted;
        }

        private void GenerateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            ObjectField objectField = new ObjectField("Behaviour Tree")
            {
                objectType = typeof(BehaviourTreeAsset),
                allowSceneObjects = false
            };
            objectField.RegisterValueChangedCallback(evt =>
            {
                _treeAsset = evt.newValue as BehaviourTreeAsset;
                _graphView.PopulateView(_treeAsset);
                _variablesOverlay.SetTreeAsset(_treeAsset);
                _variablesOverlay.Refresh();
            });
            toolbar.Add(objectField);

            rootVisualElement.Add(toolbar);

            Button saveButton = new Button(Save) { text = "Save " };
            saveButton.style.marginBottom = 6;
            toolbar.Add(saveButton);

            Button resetButton = new Button(ResetTree) { text = "Reset BT (runtime)" };
            resetButton.style.marginBottom = 6;
            toolbar.Add(resetButton);

            Button variablesButton = new Button(ToggleVariablesOverlay) { text = "Variables" };
            toolbar.Add(variablesButton);
        }

        private void OnNodeDeleted(object sender, BehaviourTreeNodeView nodeView)
        {
            _inspectorOverlay.visible = false;
        }

        private void Save()
        {
            if (_treeAsset == null || !_graphView.IsDirty)
            {
                return;
            }

            foreach (BehaviourTreeNode node in _graphView.NodeViews.Keys)
            {
                if (node == null)
                {
                    continue;
                }

                SerializedObject serializedObject = new SerializedObject(node);
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(node);
            }

            EditorUtility.SetDirty(_treeAsset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _graphView.ClearDirtyFlag();
        }

        private void ResetTree()
        {
            BehaviourTreeRunner runner = FindRunnerForTree(_treeAsset);

            if (runner == null)
            {
                Debug.LogWarning("Could not find active BehaviourTreeRunner for this tree.");
                return;
            }

            runner.ResetTree();
            Debug.Log("BehaviourTree was reset successfully.");
        }

        private BehaviourTreeRunner FindRunnerForTree(BehaviourTreeAsset asset)
        {
            if (asset == null)
            {
                return null;
            }

            foreach (BehaviourTreeRunner runner in FindObjectsOfType<BehaviourTreeRunner>())
            {
                if (runner.HasTreeAsset(asset))
                {
                    return runner;
                }
            }

            return null;
        }

        public void RefreshNodeInspector(BehaviourTreeNode node)
        {
            NodeInspectorOverlayType overlayType = NodeInspectorOverlayType.Default;

            if (node != null)
            {
                NodeInspectorOverlayAttribute attr = node.GetType().GetCustomAttribute<NodeInspectorOverlayAttribute>();
                if (attr != null)
                {
                    overlayType = attr.OverlayType;
                }
            }

            Type overlayClassType = typeof(BehaviourTreeNodeInspectorOverlay);

            switch (overlayType)
            {
                case NodeInspectorOverlayType.WithCondition:
                    overlayClassType = typeof(BehaviourTreeNodeWithConditionInspectorOverlay);
                    break;

                case NodeInspectorOverlayType.Default:
                default:
                    overlayClassType = typeof(BehaviourTreeNodeInspectorOverlay);
                    break;
            }

            bool overlayIsWrongType = (_inspectorOverlay == null) || (_inspectorOverlay.GetType() != overlayClassType);

            if (overlayIsWrongType)
            {
                if (_inspectorOverlay != null)
                {
                    rootVisualElement.Remove(_inspectorOverlay);
                }

                _inspectorOverlay = (BehaviourTreeNodeInspectorOverlay)Activator.CreateInstance(overlayClassType);
                rootVisualElement.Add(_inspectorOverlay);
            }

            _inspectorOverlay.UpdateSelection(this, node);
        }

        private void InitializeVariablesOverlay()
        {
            _variablesOverlay = new BehaviourTreeVariablesOverlay(_treeAsset, _graphView);
            _variablesOverlay.visible = false;
            rootVisualElement.Add(_variablesOverlay);
            _variablesOverlay.Refresh();
        }

        private void ToggleVariablesOverlay()
        {
            if (_variablesOverlay != null)
            {
                _variablesOverlay.visible = !_variablesOverlay.visible;
            }
        }

        private void InitializeDragAndDrop()
        {
            rootVisualElement.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                if (DragAndDrop.GetGenericData("VariableGUID") is string)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    evt.StopPropagation();
                }
            });

            rootVisualElement.RegisterCallback<DragPerformEvent>(evt =>
            {
                if (DragAndDrop.GetGenericData("VariableGUID") is not string guid)
                {
                    return;
                }

                DragAndDrop.AcceptDrag();
                evt.StopPropagation();

                Vector2 globalMousePos = evt.mousePosition;
                Vector2 localPos = _graphView.contentViewContainer.WorldToLocal(globalMousePos);

                ShowVariableDropDownMenu(guid, localPos);
            });
        }

        private void ShowVariableDropDownMenu(string variableGuid, Vector2 position)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Get Variable"), false, () =>
            {
                _graphView.CreateVariableNode(true, variableGuid, position);
            });

            menu.AddItem(new GUIContent("Set Variable"), false, () =>
            {
                _graphView.CreateVariableNode(false, variableGuid, position);
            });

            menu.DropDown(new Rect(position, Vector2.zero));
        }
    }
}