using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectSC.Saving;
using ProjectSC.Saving.SaveData;

public class SaveTestObject : MonoBehaviour, ISaveable
{
    private void Awake()
    {
        RegisterISaveable();
    }

    private void RegisterISaveable()
    {
        SaveSystem.RegisterSaveable(this);
    }

    public SaveData Save()
    {
        TransformSaveData saveData = new TransformSaveData();
        saveData.ObjType = SaveDataType.TransformSaveData;
        saveData.PositionX = transform.position.x;
        saveData.PositionY = transform.position.y;
        saveData.PositionZ = transform.position.z;
        return saveData;
    }
}
