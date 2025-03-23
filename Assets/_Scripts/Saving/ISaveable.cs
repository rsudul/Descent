using ProjectSC.Saving.SaveData;

namespace ProjectSC.Saving
{
    public interface ISaveable
    {
        SaveData.SaveData Save();
    }
}