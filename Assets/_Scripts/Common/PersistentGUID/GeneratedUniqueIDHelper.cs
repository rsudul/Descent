using UnityEngine;

namespace Descent.Common.PersistentGUID
{
    public static class GeneratedUniqueIDHelper
    {
        public static bool IsUnique(GeneratedUniqueID generatedUniqueIDToCheck)
        {
#if UNITY_EDITOR
            GeneratedUniqueID[] allGeneratedUniqueIDs = GameObject.FindObjectsOfType<GeneratedUniqueID>(true);

            foreach (GeneratedUniqueID generatedUniqueID in allGeneratedUniqueIDs)
            {
                if (generatedUniqueID == generatedUniqueIDToCheck)
                {
                    continue;
                }

                if (generatedUniqueID.UniqueID == generatedUniqueIDToCheck.UniqueID)
                {
                    return false;
                }
            }
#endif

            return true;
        }
        public static bool IsUnique(string uniqueIDToCheck)
        {
#if UNITY_EDITOR
            GeneratedUniqueID[] allGeneratedUniqueIDs = GameObject.FindObjectsOfType<GeneratedUniqueID>(true);
            int count = 0;

            foreach (GeneratedUniqueID generatedUniqueID in allGeneratedUniqueIDs)
            {
                if (generatedUniqueID.UniqueID == uniqueIDToCheck)
                {
                    count++;
                }

                if (count > 1)
                {
                    return true;
                }
            }
#endif

            return true;
        }
    }
}