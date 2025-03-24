namespace Descent.SaveSystem
{
    [System.Serializable]
    public class TransformSaveData : SerializedObjectSaveData
    {
        public float PositionX { get; set; } = 0.0f;
        public float PositionY { get; set; } = 0.0f;
        public float PositionZ { get; set; } = 0.0f;
    }
}