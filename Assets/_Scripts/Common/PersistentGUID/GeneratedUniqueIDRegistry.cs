using System.Collections.Generic;
using UnityEngine;

namespace Descent.Common.PersistentGUID
{
    public static class GeneratedUniqueIDRegistry
    {
        private static Dictionary<string, GeneratedUniqueID> _registry = new Dictionary<string, GeneratedUniqueID>();

        public static void Register(GeneratedUniqueID uidObject)
        {
            if (uidObject == null || string.IsNullOrEmpty(uidObject.UniqueID))
            {
                return;
            }

            if (!_registry.ContainsKey(uidObject.UniqueID))
            {
                _registry[uidObject.UniqueID] = uidObject;
            }
            else
            {
                Debug.LogWarning($"Duplicate UniqueID '{uidObject.UniqueID}' detected during registration.");
            }
        }

        public static void Unregister(GeneratedUniqueID uidObject)
        {
            if (uidObject == null || string.IsNullOrEmpty(uidObject.UniqueID))
            {
                return;
            }

            if (_registry.ContainsKey(uidObject.UniqueID) && _registry[uidObject.UniqueID] == uidObject)
            {
                _registry.Remove(uidObject.UniqueID);
            }
        }

        public static GeneratedUniqueID Get(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return null;
            }

            if (_registry.TryGetValue(uid, out GeneratedUniqueID retrievedGeneratedUniqueID))
            {
                return retrievedGeneratedUniqueID;
            }

            return null;
        }

        public static void Clear() => _registry.Clear();
    }
}