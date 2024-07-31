using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SaveLoadSystem : SingletonMonoBehavior<SaveLoadSystem>, IGameSystem
{
    SaveData saveData = new SaveData();

    [SerializeField] Transform gameObjectParent;

    public void LoadToSaveData()
    {
        LocalSaveSystem.Init();
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
        GameObject emptyBuilding = new GameObject
        {
            transform =
            {
                parent = gameObjectParent,
                position = data.position,
            },
           name = ResourceManager.Instance.allBuildingData[data.factoryDataID].buildingName,
        };

        Factory emptyFactory = emptyBuilding.AddComponent<Factory>();
        emptyFactory.Init(ResourceManager.Instance.allBuildingData[data.factoryDataID]);
        emptyFactory.queueCapacity = data.queueCapacity;

        Queue<SavedProcessingData> savedProcessing = new Queue<SavedProcessingData>(data.processing);
        Queue<string> savedCompleted = new Queue<string>(data.completed);
        Queue<ProductData> processingProductData = new Queue<ProductData>();
        Queue<ProductData> completedProductData = new Queue<ProductData>();

        while (savedProcessing.Peek().completedTime < DateTime.Now)
        {
            savedCompleted.Enqueue(savedProcessing.Dequeue().productDataID);
            if (savedProcessing.Count == 0) break;
        }

        foreach(SavedProcessingData processingData in savedProcessing)
        {
            processingProductData.Enqueue(ResourceManager.Instance.TranslateToProductData(processingData.productDataID));
        }
        
        foreach(string completedData in savedCompleted)
        {
            completedProductData.Enqueue(ResourceManager.Instance.TranslateToProductData(completedData));
        }
        
        BuildingSystem.Instance.ColorTileFollowBuilding(data.area);
    }

    public void LoadStorageData()
    {
        
    }
    


}
