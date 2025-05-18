using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace Descent.Common.PersistentGUID
{
    [ExecuteAlways]
    public class GeneratedUniqueID : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private string _uniqueID = string.Empty;

        public string UniqueID => _uniqueID;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(_uniqueID) || !GeneratedUniqueIDHelper.IsUnique(this))
            {
                GenerateUniqueID();
                EditorUtility.SetDirty(this);
            }
#endif
        }

        private void GenerateUniqueID()
        {
            string newUniqueID = string.Empty;
            int triesCount = 0;

            do
            {
                newUniqueID = Guid.NewGuid().ToString();
                triesCount++;
            } while (!GeneratedUniqueIDHelper.IsUnique(newUniqueID));

            _uniqueID = newUniqueID;
        }

        private void OnEnable()
        {
            GeneratedUniqueIDRegistry.Register(this);
        }

        private void OnDisable()
        {
            GeneratedUniqueIDRegistry.Unregister(this);
        }
    }
}