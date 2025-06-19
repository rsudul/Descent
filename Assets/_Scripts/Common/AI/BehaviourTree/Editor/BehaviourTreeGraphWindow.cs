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
        private VisualElement _inspectorContainer;
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

            _graphView.OnNodeDeleted += OnNodeDeleted;
        }

        private void OnDisable()
        {
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

            _inspectorContainer = new VisualElement();
            _inspectorContainer.style.position = Position.Absolute;
            _inspectorContainer.style.top = 40;
            _inspectorContainer.style.right = 10;
            _inspectorContainer.style.width = 250;
            _inspectorContainer.style.backgroundColor = new Color(0, 0, 0, 0.65f);
            _inspectorContainer.style.paddingBottom = 6;
            _inspectorContainer.style.paddingTop = 6;
            _inspectorContainer.style.paddingLeft = 8;
            _inspectorContainer.style.paddingRight = 8;
            _inspectorContainer.style.borderBottomLeftRadius = 6;
            _inspectorContainer.style.borderTopLeftRadius = 6;
            _inspectorContainer.style.borderTopRightRadius = 6;
            _inspectorContainer.style.borderBottomRightRadius = 6;
            _inspectorContainer.style.borderTopWidth = 1;
            _inspectorContainer.style.borderLeftWidth = 1;
            _inspectorContainer.style.borderRightWidth = 1;
            _inspectorContainer.style.borderBottomWidth = 1;
            _inspectorContainer.visible = false;

            rootVisualElement.Add(_inspectorContainer);
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

            _graphView.OnNodeSelected += ShowInspectorForNode;

            Button saveButton = new Button(Save) { text = "Save " };
            saveButton.style.marginBottom = 6;
            toolbar.Add(saveButton);
        }

        private void ShowInspectorForNode(object sender, BehaviourTreeNode node)
        {
            _inspectorContainer.Clear();

            if (node == null)
            {
                _inspectorContainer.visible = false;
                return;
            }

            _inspectorContainer.visible = true;

            Button closeButton = new Button(() => _inspectorContainer.visible = false)
            {
                text = "X"
            };
            closeButton.style.unityTextAlign = TextAnchor.MiddleRight;
            closeButton.style.alignSelf = Align.FlexEnd;
            _inspectorContainer.Add(closeButton);

            BehaviourTreeNodeEditorProxy proxy = BehaviourTreeNodeEditorProxy.Create(node);
            SerializedObject serializedProxy = new SerializedObject(proxy);
            SerializedProperty nodeProperty = serializedProxy.FindProperty("Node");
            string oldName = node.Name;

            IMGUIContainer container = new IMGUIContainer(() =>
            {
                serializedProxy.Update();

                EditorGUILayout.LabelField("Node Inspector", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                if (nodeProperty != null && nodeProperty.hasVisibleChildren)
                {
                    SerializedProperty child = nodeProperty.Copy();
                    SerializedProperty end = child.GetEndProperty();

                    child.NextVisible(true);

                    while (!SerializedProperty.EqualContents(child, end))
                    {
                        EditorGUILayout.PropertyField(child, true);
                        child.NextVisible(false);
                    }

                    if (node.Name != oldName)
                    {
                        _graphView.RefreshNodeTitle(node);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("No editable properties.");
                }

                serializedProxy.ApplyModifiedProperties();
            });

            _inspectorContainer.Add(container);
        }

        private void OnNodeDeleted(object sender, BehaviourTreeNodeView nodeView)
        {
            _inspectorContainer.visible = false;
        }

        private void Save()
        {
            if (_treeAsset == null || !_graphView.IsDirty)
            {
                return;
            }

            EditorUtility.SetDirty(_treeAsset);
            AssetDatabase.SaveAssets();
            _graphView.ClearDirtyFlag();
        }
    }
}