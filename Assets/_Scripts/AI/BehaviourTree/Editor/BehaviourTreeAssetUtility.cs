using Descent.AI.BehaviourTree.Core;
using Descent.AI.BehaviourTree.Nodes;
using UnityEditor;
using UnityEngine;

public class BehaviourTreeAssetUtility : MonoBehaviour
{
    [MenuItem("Tools/Descent/Debug Selected Behaviour Tree")]
    public static void DebugSelectedBehaviourTree()
    {
        UnityEngine.Object[] selected = Selection.objects;

        foreach (var obj in selected)
        {
            if (obj is BehaviourTreeAsset asset)
            {
                Debug.Log($"=== Debugging {asset.name} ===");

                string assetPath = AssetDatabase.GetAssetPath(asset);
                UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

                Debug.Log($"Total sub-assets: {subAssets.Length}");

                foreach (var subAsset in subAssets)
                {
                    if (subAsset is BehaviourTreeNode node)
                    {
                        Debug.Log($"Node: '{subAsset.name}' | Type: {subAsset.GetType().Name} | GUID: '{node.GUID}' | Custom Name: '{node.Name}'");
                    }
                    else
                    {
                        Debug.Log($"Other: '{subAsset.name}' | Type: {subAsset.GetType().Name}");
                    }
                }
            }
        }
    }
    [MenuItem("Tools/Descent/Debug Asset Contents")]
    public static void DebugAssetContents()
    {
        UnityEngine.Object[] selected = Selection.objects;

        foreach (var obj in selected)
        {
            if (obj is BehaviourTreeAsset asset)
            {
                Debug.Log($"=== Asset Contents for {asset.name} ===");

                string assetPath = AssetDatabase.GetAssetPath(asset);
                UnityEngine.Object[] allSubAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

                Debug.Log($"Total objects in asset file: {allSubAssets.Length}");

                foreach (var subAsset in allSubAssets)
                {
                    string type = subAsset.GetType().Name;
                    string name = subAsset.name;
                    string guid = "";

                    if (subAsset is BehaviourTreeNode node)
                    {
                        guid = $" | GUID: {node.GUID}";
                    }

                    Debug.Log($"  - {name} ({type}){guid}");
                }

                Debug.Log($"=== Asset._allNodes count: {asset.AllNodes.Count} ===");
                foreach (var node in asset.AllNodes)
                {
                    Debug.Log($"  - _allNodes: {node.Name} ({node.GetType().Name}) | GUID: {node.GUID}");
                }
            }
        }
    }
}
