using System;

namespace Descent.Common.PersistentGUID
{
    public static class UniqueIDGenerator
    {
        public static string GenerateUniqueID()
        {
#if UNITY_EDITOR
            string newUniqueID = string.Empty;

            do
            {
                newUniqueID = Guid.NewGuid().ToString();
            } while (!IsUnique(newUniqueID));

            return newUniqueID;
#else
            return string.Empty;
#endif
        }

        public static bool IsUnique(GeneratedUniqueID idToCheck)
        {
#if UNITY_EDITOR
            if (idToCheck == null || string.IsNullOrEmpty(idToCheck.UniqueID))
            {
                return false;
            }

            GeneratedUniqueID existingObject = GeneratedUniqueIDRegistry.Get(idToCheck.UniqueID);

            return existingObject == null || existingObject == idToCheck;
#else
            return true;
#endif
        }

        public static bool IsUnique(string uniqueIDToCheck)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(uniqueIDToCheck))
            {
                return false;
            }

            GeneratedUniqueID existingObject = GeneratedUniqueIDRegistry.Get(uniqueIDToCheck);

            return existingObject == null;
#else
            return true;
#endif
        }
    }
}