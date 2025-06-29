using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Descent.Common.AI.BehaviourTree.Core;
using UnityEditor.UIElements;
using System;
using Descent.Common.AI.BehaviourTree.Nodes;

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

            _graphView.OnNodeSelected -= _inspectorOverlay.UpdateSelection;
            _graphView.OnNodeDeleted -= OnNodeDeleted;
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

            _inspectorOverlay = new BehaviourTreeNodeInspectorOverlay();
            rootVisualElement.Add(_inspectorOverlay);

            _graphView.OnNodeSelected += _inspectorOverlay.UpdateSelection;
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

            foreach (var node in _graphView.NodeViews.Keys)
            {
                if (node == null)
                {
                    continue;
                }

                EditorUtility.SetDirty(node);
            }

            EditorUtility.SetDirty(_treeAsset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _graphView.ClearDirtyFlag();
        }
    }
}