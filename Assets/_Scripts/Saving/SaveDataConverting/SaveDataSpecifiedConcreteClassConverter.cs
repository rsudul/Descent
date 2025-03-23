using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace ProjectSC.Saving.SaveDataConverting
{
    public class SaveDataSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(SaveData.SaveData).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            {
                return null;
            }
            return base.ResolveContractConverter(objectType);
        }
    }
}