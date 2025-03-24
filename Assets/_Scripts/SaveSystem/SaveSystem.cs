using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

namespace Descent.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        private static Dictionary<string, ISaveable> _saveableObjects = new Dictionary<string, ISaveable> ();

        public static void RegisterSaveable(string guid, ISaveable saveable)
        {
            _saveableObjects.Add(guid, saveable);
        }

        public static SaveSystemOperationResult Save()
        {
            SaveSystemOperationResult saveResult = new SaveSystemOperationResult();

            try
            {
                string saveDataFileName = "SaveDataFile_" + System.DateTime.Now;
                string saveFilePath = "SaveDataFile.json";
                SaveDataFile saveDataFile = new SaveDataFile();
                saveDataFile.Filename = saveDataFileName;
                List<SaveData> saveDataList = new List<SaveData>();

                foreach (KeyValuePair<string, ISaveable> saveable in _saveableObjects)
                {
                    SaveData saveData = saveable.Value.Save();
                    saveDataList.Add(saveData);
                }

                saveDataFile.SaveData = saveDataList.ToArray();

                string saveDataJson = JsonConvert.SerializeObject(saveDataFile);

                File.WriteAllText(saveFilePath, saveDataJson);
            } catch (Exception e)
            {
                saveResult.Successful = false;
                saveResult.Exception = e.Message;
                return saveResult;
            }

            saveResult.Successful = true;

            return saveResult;
        }

        public static SaveSystemOperationResult Load()
        {
            SaveSystemOperationResult loadResult = new SaveSystemOperationResult();

            try
            {
                string saveFilePath = "SaveDataFile.json";
                SaveDataFile saveDataFile = new SaveDataFile();

                string saveDataString = File.ReadAllText(saveFilePath);
                saveDataFile = JsonConvert.DeserializeObject<SaveDataFile>(saveDataString);

                foreach (SaveData saveData in saveDataFile.SaveData)
                {
                    if (saveData is not SerializedObjectSaveData serializedObjectSaveData)
                    {
                        continue;
                    }

                    _saveableObjects[serializedObjectSaveData.Guid].Load(saveData);
                }
            } catch (Exception e)
            {
                loadResult.Successful = false;
                loadResult.Exception = e.Message;
                return loadResult;
            }

            loadResult.Successful = true;
            return loadResult;
        }
    }
}