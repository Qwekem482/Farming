using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SavedFactoryData
{
    public string factoryID;
    public string factoryDataID;
    public Vector3 position;
    public BoundsInt area;
    public int queueCapacity;
    public Queue<SavedProcessingData> processing; 
    public Queue<string> completed; //productDataID of completed item

    public SavedFactoryData(string factoryID, string factoryDataID, Vector3 position, 
        BoundsInt area, int queueCapacity, Queue<SavedProcessingData> processing, 
        Queue<string> completed)
    {
        this.factoryID = factoryID;
        this.factoryDataID = factoryDataID;
        this.position = position;
        this.area = area;
        this.queueCapacity = queueCapacity;
        this.processing = processing;
        this.completed = completed;
    }
}

[Serializable]
public class SavedProcessingData
{
    public string productDataID;
    public DateTime completedTime; //TimeSpan => CompletedTime

    public SavedProcessingData(string productDataID, DateTime completedTime)
    {
        this.productDataID = productDataID;
        this.completedTime = completedTime;
    }
}
