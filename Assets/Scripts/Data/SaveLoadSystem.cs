using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : SingletonMonoBehavior<SaveLoadSystem>, IGameSystem
{
    SaveData saveData = new SaveData();

    void LoadToSaveData()
    {
        saveData = LocalSaveSystem.LoadData<SaveData>();
    }

    //Actually not start here, just AddListener to receive data to save (to SaveData object)
    public void StartingSystem()
    {
        EventManager.Instance.AddListener<FactoryDataEvent>(saveData.AddFactoryData);
    }
}
