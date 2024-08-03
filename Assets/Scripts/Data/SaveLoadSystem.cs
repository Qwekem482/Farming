using System;
using System.Collections.Generic;
using UnityEngine;


public class SaveLoadSystem : SingletonMonoBehavior<SaveLoadSystem>, IGameSystem
{
    SaveData saveData = new SaveData();

    [SerializeField] Transform gameObjectParent;

    public void LoadToSaveData()
    {
        saveData = LocalSaveSystem.LoadData<SaveData>();
    }

    //Actually not start here, just AddListener to receive data to save (to SaveData object)
    public void StartingSystem()
    {
        EventManager.Instance.AddListener<FactoryDataEvent>(saveData.AddFactoryData);
    }

    public void LoadFactoryData()
    {
        foreach(SavedFactoryData savedFactoryData in saveData.factoryData.Values)
        {
            CreateFactory(savedFactoryData);
        }
    }

    void CreateFactory(SavedFactoryData data)
    {
        GameObject emptyBuilding = CreateGameObject(
            ResourceManager.Instance.allBuildingData[data.factoryDataID].buildingName,
            data.position,
            data.area);
        
        Factory emptyFactory = emptyBuilding.AddComponent<Factory>();
        emptyFactory.Init(ResourceManager.Instance.allBuildingData[data.factoryDataID]);
        emptyFactory.queueCapacity = data.queueCapacity;

        Queue<SavedProcessingData> savedProcessing = new Queue<SavedProcessingData>(data.processing);
        Queue<string> savedCompleted = new Queue<string>(data.completed);
        Queue<ProductData> processingProductData = new Queue<ProductData>();
        Queue<ProductData> completedProductData = new Queue<ProductData>();

        TimeSpan difference = default;
        while (savedProcessing.Peek().completedTime < DateTime.Now)
        {
            savedCompleted.Enqueue(savedProcessing.Dequeue().productDataID);
            if (savedProcessing.Count == 0) break;
            difference = DateTime.Now - savedProcessing.Peek().completedTime;
        }

        foreach(SavedProcessingData processingData in savedProcessing)
        {
            processingProductData.Enqueue(ResourceManager.Instance.TranslateToProductData(processingData.productDataID));
        }
        
        foreach(string completedData in savedCompleted)
        {
            completedProductData.Enqueue(ResourceManager.Instance.TranslateToProductData(completedData));
        }
        
        emptyFactory.LoadFactory(data.factoryID, processingProductData, completedProductData, difference);
    }

    GameObject CreateGameObject(string objectName, Vector3 position, BoundsInt area)
    {
        GameObject emptyBuilding = new GameObject
        {
            transform =
            {
                parent = gameObjectParent,
                position = position,
            },
            name = objectName,
        };
        
        BuildingSystem.Instance.ColorTileFollowBuilding(area);

        return emptyBuilding;
    }

    void OnApplicationPause(bool pauseStatus)
    {
        SaveData();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }

    void SaveData()
    {
        LocalSaveSystem.Save(saveData);
    }
}
