using System;
using System.Collections.Generic;
using UnityEngine;


public class SaveLoadSystem : SingletonMonoBehavior<SaveLoadSystem>, IGameSystem
{
    SaveData saveData = new SaveData();
    bool isLoaded;

    [SerializeField] Transform gameObjectParent;

    public void LoadToSaveData()
    {
        saveData = LocalSaveSystem.LoadData<SaveData>() ?? new SaveData();
    }

    //Actually not start here, just AddListener to receive data to save (to SaveData object)
    public void StartingSystem()
    {
        Debug.Log("saveData : " + saveData);
        EventManager.Instance.AddListener<SaveFactoryDataEvent>(saveData.AddFactoryData);
    }

    public void LoadAllData()
    {
        LoadFactoryData();
    }

    void LoadFactoryData()
    {
        isLoaded = true;
        if (saveData == null) return;
        foreach(SavedFactoryData savedFactoryData in saveData.factoryData.Values)
        {
            CreateFactory(savedFactoryData);
        }
    }

    void CreateFactory(SavedFactoryData data)
    {
        BuildingData buildingData = ResourceManager.Instance.allBuildingData[data.buildingDataID];
        GameObject emptyBuilding = CreateBuildingGameObject(
            buildingData.buildingName,
            data.position,
            data.area,
            buildingData.sprite);
        
        Factory emptyFactory = emptyBuilding.AddComponent<Factory>();
        emptyFactory.Init(buildingData);
        emptyFactory.queueCapacity = data.queueCapacity;
        
        Queue<ProductData>[] productDataQueue = InitializeProductionQueue
        (new Queue<SavedProcessingData>(data.processing),
            new Queue<string>(data.completed));

        TimeSpan difference = default;
        emptyFactory.LoadState(data.buildingID, data.queueCapacity, 
            productDataQueue[0],productDataQueue[1], difference);
    }

    void CreateField(SavedFieldData data)
    {
        BuildingData buildingData = ResourceManager.Instance.allBuildingData[data.buildingDataID];
        GameObject emptyBuilding = CreateBuildingGameObject(
            buildingData.buildingName,
            data.position,
            data.area,
            buildingData.sprite);

        Field emptyField = emptyBuilding.AddComponent<Field>();
        emptyField.Init(buildingData);
    }

    GameObject CreateBuildingGameObject(string objectName, Vector3 position, BoundsInt area, Sprite sprite)
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
        
        emptyBuilding.AddComponent<SpriteRenderer>().sprite = sprite;
        BuildingSystem.Instance.ColorTileFollowBuilding(area);

        return emptyBuilding;
    }

    Queue<ProductData>[] InitializeProductionQueue(Queue<SavedProcessingData> savedProcessing, Queue<string> savedCompleted)
    {
        Queue<ProductData> processingProductData = new Queue<ProductData>();
        Queue<ProductData> completedProductData = new Queue<ProductData>();
        
        while (savedProcessing.Count != 0 && savedProcessing.Peek().completedDateTime < DateTime.Now)
        {
            savedCompleted.Enqueue(savedProcessing.Dequeue().productDataID);
            if (savedProcessing.Count == 0) break;
        }

        if (savedProcessing.Count != 0)
        {
            foreach(SavedProcessingData processingData in savedProcessing)
            {
                processingProductData.Enqueue(ResourceManager.Instance.
                    TranslateToProductData(processingData.productDataID));
            }
        }
        
        if (completedProductData.Count != 0)
        {
            foreach(string completedData in savedCompleted)
            {
                completedProductData.Enqueue(ResourceManager.Instance.
                    TranslateToProductData(completedData));
            }
        }

        return new[] { processingProductData, completedProductData };
    }

    /*void OnApplicationPause(bool pauseStatus)
    {
        if (isLoaded) SaveData();
    }*/

    void OnApplicationQuit()
    {
        if (isLoaded) SaveData();
    }

    void SaveData()
    {
        LocalSaveSystem.Save(saveData);
    }
}
