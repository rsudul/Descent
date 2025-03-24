namespace Descent.SaveSystem
{
    [System.Serializable]
    public class SaveDataFile
    {
        public SaveData[] SaveData { get; set; }
        public string Filename { get; set; }
    }
}