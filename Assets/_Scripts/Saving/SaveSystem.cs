using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ProjectSC.Saving.SaveData;
using System;

namespace ProjectSC.Saving
{
    public class SaveSystem : MonoBehaviour
    {
        private static HashSet<ISaveable> _saveableObjects = new HashSet<ISaveable>();

        public static void RegisterSaveable(ISaveable saveable)
        {
            _saveableObjects.Add(saveable);
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
                List<SaveData.SaveData> saveDataList = new List<SaveData.SaveData>();

                foreach (ISaveable saveable in _saveableObjects)
                {
                    SaveData.SaveData saveData = saveable.Save();
                    saveDataList.Add(saveData);
                }

                saveDataFile.SaveData = saveDataList.ToArray();

                string saveDataJson = JsonConvert.SerializeObject(saveDataFile);

                File.WriteAllText(saveFilePath, saveDataJson);
            } catch (Exception e)
            {
                saveResult.Successfull = false;
                saveResult.Exception = e.Message;
                return saveResult;
            }

            saveResult.Successfull = true;

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

                foreach (SaveData.SaveData saveData in saveDataFile.SaveData)
                {
                    Debug.Log(((TransformSaveData)saveData).PositionX);
                }
            } catch (Exception e)
            {
                loadResult.Successfull = false;
                loadResult.Exception = e.Message;
                Debug.LogError(e.Message);
                return loadResult;
            }

            loadResult.Successfull = true;
            return loadResult;
        }
    }
}