using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Descent.Common.PersistentGUID.Validation.Data;
using UnityEditor.SceneManagement;

namespace Descent.Common.PersistentGUID.Validation
{
    public class UniqueIDProjectValidator : IUniqueIDProjectValidator
    {
        public event Action<ValidationResult> OnValidationCompleted;

        private List<GeneratedUniqueID> _allMissingIDs = new List<GeneratedUniqueID>();
        private List<GeneratedUniqueID> _allDuplicateIDs = new List<GeneratedUniqueID>();

        public void ValidateProject(IUniqueIDSceneValidator sceneValidator)
        {
            _allMissingIDs.Clear();
            _allDuplicateIDs.Clear();

            string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");

            for (int i = 0; i < sceneGuids.Length; i++)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
                Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

                ValidationResult result = sceneValidator.ValidateScene(scene);

                _allMissingIDs.AddRange(result.MissingIDs);
                _allDuplicateIDs.AddRange(result.DuplicateIDs);

                EditorSceneManager.CloseScene(scene, true);
            }

            ValidationResult finalResult = new ValidationResult(_allMissingIDs, _allDuplicateIDs);
            OnValidationCompleted?.Invoke(finalResult);
        }
    }
}