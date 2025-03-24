using Newtonsoft.Json;
using ProjectSC.SaveSystem.SaveDataConverting;

namespace ProjectSC.SaveSystem
{
    [JsonConverter(typeof(SaveDataConverter))]
    public abstract class SaveData
    {
        public SaveDataType ObjType { get; set; }
    }
}