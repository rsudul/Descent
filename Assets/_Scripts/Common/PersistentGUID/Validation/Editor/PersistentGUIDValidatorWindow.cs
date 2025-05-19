using Descent.Common.PersistentGUID.Validation.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descent.Common.PersistentGUID.Validation.Editor
{
    public class PersistentGUIDValidatorWindow : EditorWindow
    {
        private IUniqueIDSceneValidator _sceneValidator;
        private IUniqueIDProjectValidator _projectValidator;
        private IUniqueIDAutoFixer _autoFixer;

        private ValidationResult _lastValidationResult;

        [MenuItem("Tools/Persistent GUID Validator")]
        public static void ShowWindow()
        {
            PersistentGUIDValidatorWindow window = GetWindow<PersistentGUIDValidatorWindow>("GUID Validator");
            window.Initialize();
        }

        private void Initialize()
        {
            _sceneValidator = new UniqueIDSceneValidator();
            _projectValidator = new UniqueIDProjectValidator();
            _autoFixer = new UniqueIDAutoFixer();

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            if (_sceneValidator != null)
            {
                _sceneValidator.OnValidationCompleted += OnSceneValidationCompleted;
            }

            if (_projectValidator != null)
            {
                _projectValidator.OnValidationCompleted += OnProjectValidationCompleted;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_sceneValidator != null)
            {
                _sceneValidator.OnValidationCompleted -= OnSceneValidationCompleted;
            }

            if (_projectValidator != null)
            {
                _projectValidator.OnValidationCompleted -= OnProjectValidationCompleted;
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Validate Active Scene"))
            {
                _sceneValidator.ValidateScene(SceneManager.GetActiveScene());
            }

            if (GUILayout.Button("Validate Project"))
            {
                _projectValidator.ValidateProject(_sceneValidator);
            }

            if (_lastValidationResult != null)
            {
                GUILayout.Label($"Missing IDs: {_lastValidationResult.MissingIDs.Count}");
                GUILayout.Label($"Duplicate IDs: {_lastValidationResult.DuplicateIDs.Count}");

                if (_lastValidationResult.MissingIDs.Count > 0 && GUILayout.Button("Fix Missing IDs"))
                {
                    _autoFixer.FixMissingIDs(_lastValidationResult.MissingIDs);
                }

                if (_lastValidationResult.DuplicateIDs.Count > 0 && GUILayout.Button("Fix Duplicate IDs"))
                {
                    _autoFixer.FixDuplicateIDs(_lastValidationResult.DuplicateIDs);
                }
            }
        }

        private void OnSceneValidationCompleted(ValidationResult result)
        {
            Debug.Log("Unique IDs validation completed!");

            _lastValidationResult = result;
            Repaint();
        }

        private void OnProjectValidationCompleted(ValidationResult result)
        {
            Debug.Log("Project validation completed!");

            _lastValidationResult = result;
            Repaint();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }
    }
}