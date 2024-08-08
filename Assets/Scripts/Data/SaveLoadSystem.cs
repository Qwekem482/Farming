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
        EventManager.Instance.AddListener<SaveFactoryDataEvent>(saveData.AddFactoryData);
        EventManager.Instance.AddListener<SaveFieldDataEvent>(saveData.AddFieldData);
    }

    #region LoadSave

    public void LoadAllData()
    {
        isLoaded = true;
        if (saveData == null) return;
        
        LoadFactoryData();
        LoadFieldData();
    }

    void LoadFactoryData()
    {
        foreach(SavedFactoryData savedFactoryData in saveData.factoryData.Values)
        {
            CreateFactory(savedFactoryData);
        }
    }

    void LoadFieldData()
    {
        foreach(SavedFieldData savedFieldData in saveData.fieldData.Values)
        {
            CreateField(savedFieldData);
        }
    }

    #endregion

    #region CreateGameObject

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

        TimeSpan difference = TimeDifference(new Queue<SavedProcessingData>(data.processing));;
        
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

        TimeSpan difference = default;
        if (data.processingData.completedDateTime > DateTime.Now) 
            difference = data.processingData.completedDateTime - DateTime.Now;
        
        emptyField.LoadState(
            data.buildingID,
            ResourceManager.Instance.TranslateToProductData
                (data.processingData.productDataID) as CropData,
            difference);
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

    #endregion

    #region CreateFactorySupport
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
                    TranslateToProductData(processingData.productDataID) as ProductData);
            }
        }
        
        if (completedProductData.Count != 0)
        {
            foreach(string completedData in savedCompleted)
            {
                completedProductData.Enqueue(ResourceManager.Instance.
                    TranslateToProductData(completedData) as ProductData);
            }
        }

        return new[] { processingProductData, completedProductData };
    }

    TimeSpan TimeDifference(Queue<SavedProcessingData> savedProcessing)
    {
        TimeSpan toReturn = default;
        while (savedProcessing.Count != 0 && savedProcessing.Peek().completedDateTime < DateTime.Now)
        {
            toReturn = savedProcessing.Dequeue().completedDateTime - DateTime.Now;
            
            if (savedProcessing.Count == 0)
            {
                toReturn = default;
                break;
            }
        }

        return toReturn;
    }

    #endregion

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
