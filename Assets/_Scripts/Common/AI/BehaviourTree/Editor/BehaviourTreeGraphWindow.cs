using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.Common.AI.BehaviourTree.Core;
using UnityEditor.UIElements;
using System;
using System.Reflection;
using Descent.Common.AI.BehaviourTree.Nodes;
using Descent.Common.Attributes.AI;

namespace Descent.Common.AI.BehaviourTree.Editor
{
    public class BehaviourTreeGraphWindow : EditorWindow
    {
        private BehaviourTreeGraphView _graphView;
        private BehaviourTreeNodeInspectorOverlay _inspectorOverlay;
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
            });
            toolbar.Add(objectField);

            rootVisualElement.Add(toolbar);

            Button saveButton = new Button(Save) { text = "Save " };
            saveButton.style.marginBottom = 6;
            toolbar.Add(saveButton);

            Button resetButton = new Button(ResetTree) { text = "Reset BT (runtime)" };
            resetButton.style.marginBottom = 6;
            toolbar.Add(resetButton);
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
    }
}