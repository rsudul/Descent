using Descent.Common.ObjectSpawning.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace Descent.Common.ObjectSpawning
{
    public class ObjectSpawner : MonoBehaviour
    {
        private static Dictionary<ObjectSpawnType, GameObject> objectTypeDictionary = new Dictionary<ObjectSpawnType, GameObject>();

        [SerializeField]
        private ObjectSpawnerSettings _settings;

        private void Awake()
        {
            foreach (ObjectSpawnWrapper wrapper in _settings.ObjectSpawnWrapper)
            {
                if (objectTypeDictionary.ContainsKey(wrapper.SpawnType))
                {
                    continue;
                }

                objectTypeDictionary.Add(wrapper.SpawnType, wrapper.SpawnObject);
            }
        }

        public static GameObject SpawnObject(ObjectSpawnType objectType)
        {
            if (!objectTypeDictionary.ContainsKey(objectType))
            {
                return null;
            }

            return Instantiate(objectTypeDictionary[objectType]);
        }

        public static GameObject SpawnObjectAtPosition(ObjectSpawnType objectType, Vector3 spawnPosition)
        {
            if (!objectTypeDictionary.ContainsKey(objectType))
            {
                return null;
            }

            return Instantiate(objectTypeDictionary[objectType], spawnPosition, Quaternion.identity);
        }
    }
}