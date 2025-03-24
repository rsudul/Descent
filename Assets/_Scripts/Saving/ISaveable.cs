namespace ProjectSC.SaveSystem
{
    public interface ISaveable
    {
        SaveData Save();
        void Load(SaveData saveData);
    }
}