using Newtonsoft.Json;
using Descent.SaveSystem.SaveDataConverting;

namespace Descent.SaveSystem
{
    [JsonConverter(typeof(SaveDataConverter))]
    public abstract class SaveData
    {
        public SaveDataType ObjType { get; set; }
    }
}