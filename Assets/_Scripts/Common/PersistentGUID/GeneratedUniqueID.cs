using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Descent.Common.PersistentGUID
{
    [ExecuteAlways]
    public class GeneratedUniqueID : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private string _uniqueID = string.Empty;

        public string UniqueID => _uniqueID;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                return;
            }

            if (string.IsNullOrEmpty(_uniqueID) || !UniqueIDGenerator.IsUnique(this))
            {
                AssignNewUniqueID();
            }
        }
#endif

        private void OnEnable()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                return;
            }

            GeneratedUniqueIDRegistry.Register(this);
        }

        private void OnDestroy()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            GeneratedUniqueIDRegistry.Unregister(this);
        }

        public void AssignNewUniqueID()
        {
            _uniqueID = UniqueIDGenerator.GenerateUniqueID();

            EditorUtility.SetDirty(this);
            GeneratedUniqueIDRegistry.Register(this);
        }
    }
}