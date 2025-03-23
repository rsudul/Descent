using Newtonsoft.Json;
using ProjectSC.Saving.SaveDataConverting;

namespace ProjectSC.Saving.SaveData
{
    [JsonConverter(typeof(SaveDataConverter))]
    public abstract class SaveData
    {
        public SaveDataType ObjType { get; set; }
    }
}