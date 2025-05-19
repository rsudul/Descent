using Descent.Common.PersistentGUID.Validation.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Descent.Common.PersistentGUID.Validation
{
    public class UniqueIDSceneValidator : IUniqueIDSceneValidator
    {
        public event Action<ValidationResult> OnValidationCompleted;

        public ValidationResult ValidateScene(Scene scene)
        {
            List<GeneratedUniqueID> missing = new List<GeneratedUniqueID>();
            List<GeneratedUniqueID> duplicates = new List<GeneratedUniqueID>();

            Dictionary<string, GeneratedUniqueID> allUIDs = new Dictionary<string, GeneratedUniqueID>();

            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject rootObj in rootObjects)
            {
                GeneratedUniqueID[] uidComponents = rootObj.GetComponentsInChildren<GeneratedUniqueID>(true);

                foreach (GeneratedUniqueID uid in uidComponents)
                {
                    if (string.IsNullOrEmpty(uid.UniqueID))
                    {
                        missing.Add(uid);
                    }
                    else if (allUIDs.ContainsKey(uid.UniqueID))
                    {
                        duplicates.Add(uid);
                    }
                    else
                    {
                        allUIDs[uid.UniqueID] = uid;
                    }
                }
            }

            ValidationResult result = new ValidationResult(missing, duplicates);

            OnValidationCompleted?.Invoke(result);

            return result;
        }
    }
}