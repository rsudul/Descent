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
            else if (!UniqueIDGenerator.IsUnique(generatedUniqueID))
            {
                EditorGUILayout.HelpBox("This UniqueID is a duplicate!", MessageType.Error);
            }

            GUI.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;
            if (GUILayout.Button("Regenerate ID"))
            {
                Undo.RecordObject(target, "Regenerate Unique ID");
                generatedUniqueID.AssignNewUniqueID();
            }
            GUI.enabled = true;
        }
    }
}