using System.Collections.Generic;
using UnityEngine;

namespace ProjectSC.Saving
{
    public class SaveSystem : MonoBehaviour
    {
        private static HashSet<ISaveable> _saveableObjects = new HashSet<ISaveable>();

        public static void RegisterSaveable(ISaveable saveable)
        {
            _saveableObjects.Add(saveable);
        }
    }
}