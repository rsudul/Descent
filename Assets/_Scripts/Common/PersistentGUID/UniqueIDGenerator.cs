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
            } while (!UniqueIDGenerator.IsUnique(newUniqueID));

            return newUniqueID;
#endif

            return string.Empty;
        }

        public static bool IsUnique(GeneratedUniqueID generatedUniqueIDToCheck)
        {
#if UNITY_EDITOR
            if (generatedUniqueIDToCheck == null || string.IsNullOrEmpty(generatedUniqueIDToCheck.UniqueID))
            {
                return false;
            }

            GeneratedUniqueID registryObject = GeneratedUniqueIDRegistry.Get(generatedUniqueIDToCheck.UniqueID);

            return registryObject == null || registryObject == generatedUniqueIDToCheck;
#endif

            return true;
        }
        public static bool IsUnique(string uniqueIDToCheck)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(uniqueIDToCheck))
            {
                return false;
            }

            GeneratedUniqueID registryObject = GeneratedUniqueIDRegistry.Get(uniqueIDToCheck);

            return registryObject == null;
#endif

            return true;
        }
    }
}