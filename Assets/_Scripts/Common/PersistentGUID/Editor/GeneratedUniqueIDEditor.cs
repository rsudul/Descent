using System.Xml;
using UnityEditor;
using UnityEngine;

namespace Descent.Common.PersistentGUID.Editor
{
    [CustomEditor(typeof(GeneratedUniqueID))]
    public class GeneratedUniqueIDEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GeneratedUniqueID generatedUniqueID = (GeneratedUniqueID)target;

            EditorGUILayout.LabelField("Unique ID", generatedUniqueID.UniqueID);

            if (string.IsNullOrEmpty(generatedUniqueID.UniqueID))
            {
                EditorGUILayout.HelpBox("UniqueID is empty.", MessageType.Warning);
            }
            else if (!GeneratedUniqueIDHelper.IsUnique(generatedUniqueID))
            {
                EditorGUILayout.HelpBox("This UniqueID is a duplicate!", MessageType.Error);
            }
        }
    }
}