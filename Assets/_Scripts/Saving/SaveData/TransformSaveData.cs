using UnityEngine;

namespace ProjectSC.Saving.SaveData
{
    [System.Serializable]
    public class TransformSaveData : SaveData
    {
        public float PositionX { get; set; } = 0.0f;
        public float PositionY { get; set; } = 0.0f;
        public float PositionZ { get; set; } = 0.0f;
    }
}