using UnityEngine;
using Descent.AI.BehaviourTree.Nodes;
using Descent.AI.BehaviourTree.Variables;
using System.Collections.Generic;
using UnityEditor;

namespace Descent.AI.BehaviourTree.Core
{
    [CreateAssetMenu(menuName = "Descent/AI/BehaviourTree")]
    public class BehaviourTreeAsset : ScriptableObject
    {
        [SerializeField]
        public BehaviourTreeNode Root;

        [SerializeField]
        private VariableContainer _variableContainer = new VariableContainer();

        [SerializeField]
        private List<ValueConnection> _valueConnections = new List<ValueConnection>();

        [SerializeField]
        private List<BehaviourTreeNode> _allNodes = new List<BehaviourTreeNode>();

        public VariableContainer VariableContainer => _variableContainer;

        public IReadOnlyList<ValueConnection> ValueConnections => _valueConnections;

        public IReadOnlyList<BehaviourTreeNode> AllNodes => _allNodes;

        public BehaviourTreeNode CloneTree()
        {
            return Root.CloneNode();
        }

        private void OnEnable()
        {
            if (_variableContainer == null)
            {
                _variableContainer = new VariableContainer();
            }

#if UNITY_EDITOR
            SyncAllNodes();
#endif
        }

        private void OnValidate()
        {
            if (_variableContainer == null)
            {
                _variableContainer = new VariableContainer();
            }

#if UNITY_EDITOR
            SyncAllNodes();
#endif
        }

        public void AddValueConnection(ValueConnection valueConnection)
        {
            _valueConnections.Add(valueConnection);
        }

        public void RemoveValueConnection(ValueConnection valueConnection)
        {
            _valueConnections.RemoveAll(c =>
            c.SourceNodeGUID == valueConnection.SourceNodeGUID &&
            c.SourcePinName == valueConnection.SourcePinName &&
            c.TargetNodeGUID == valueConnection.TargetNodeGUID &&
            c.TargetPinName == valueConnection.TargetPinName);
        }

        public void SyncAllNodes()
        {
#if UNITY_EDITOR
            _allNodes.Clear();

            if (Root != null)
            {
                CollectRecursive(Root);
            }

            CollectAllSubAssets();

            EditorUtility.SetDirty(this);
#endif
        }

        private void CollectRecursive(BehaviourTreeNode node)
        {
#if UNITY_EDITOR
            _allNodes.Add(node);

            if (node is BehaviourTreeCompositeNode compositeNode)
            {
                foreach (BehaviourTreeNode child in compositeNode.Children)
                {
                    CollectRecursive(child);
                }
            }
            else if (node is BehaviourTreeRepeatUntilFailureNode repeatUntilFailureNode)
            {
                if (repeatUntilFailureNode.Child != null)
                {
                    CollectRecursive(repeatUntilFailureNode.Child);
                }
            }
#endif
        }

        private void CollectAllSubAssets()
        {
#if UNITY_EDITOR
            string assetPath = AssetDatabase.GetAssetPath(this);
            Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach (Object subAsset in subAssets)
            {
                if (subAsset is BehaviourTreeNode node && subAsset != this)
                {
                    if (!string.IsNullOrEmpty(node.GUID) && !_allNodes.Contains(node))
                    {
                        _allNodes.Add(node);
                    }
                }
            }
#endif
        }

        [ContextMenu("Clean Up Asset")]
        public void CleanUpAsset()
        {
#if UNITY_EDITOR
            Debug.Log("Starting asset cleanup...");

            string assetPath = AssetDatabase.GetAssetPath(this);
            UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            List<UnityEngine.Object> toRemove = new List<UnityEngine.Object>();

            foreach (UnityEngine.Object subAsset in subAssets)
            {
                if (subAsset == this) continue; // Don't remove the main asset

                if (subAsset is BehaviourTreeNode node)
                {
                    // Remove nodes with empty names or invalid GUIDs
                    if (string.IsNullOrEmpty(node.name) || string.IsNullOrEmpty(node.GUID))
                    {
                        Debug.Log($"Marking for removal: {node.name} ({node.GetType().Name}) - GUID: {node.GUID}");
                        toRemove.Add(subAsset);
                    }
                }
            }

            foreach (var obj in toRemove)
            {
                AssetDatabase.RemoveObjectFromAsset(obj);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Cleanup complete. Removed {toRemove.Count} corrupted nodes.");
#endif
        }
    }
}