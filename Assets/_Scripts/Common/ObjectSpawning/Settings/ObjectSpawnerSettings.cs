using System.Collections.Generic;
using UnityEngine;

namespace ProjectSC.Common.ObjectSpawning.Settings
{
    [CreateAssetMenu(fileName = "ObjectSpawnerSettings", menuName = "Descent/ObjectSpawning/Settings")]
    public class ObjectSpawnerSettings : ScriptableObject
    {
        public List<ObjectSpawnWrapper> ObjectSpawnWrapper = new List<ObjectSpawnWrapper>();
    }
}