using UnityEngine;

namespace Descent.Gameplay.Systems.ObjectSpawning
{
    [System.Serializable]
    public struct ObjectSpawnWrapper
    {
        public ObjectSpawnType SpawnType;
        public GameObject SpawnObject;
    }
}