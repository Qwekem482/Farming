using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct SavedFactoryData
{
    public string factoryID;
    public string factoryDataID;
    public BoundsInt area;
    public int queueCapacity;
    public Queue<SavedProcessingData> processing; 
    public Queue<string> completed; //productDataID of completed item

    public SavedFactoryData(string factoryID, string factoryDataID, BoundsInt area, int queueCapacity, 
        Queue<SavedProcessingData> processing, Queue<string> completed)
    {
        this.factoryID = factoryID;
        this.factoryDataID = factoryDataID;
        this.area = area;
        this.queueCapacity = queueCapacity;
        this.processing = processing;
        this.completed = completed;
    }
}

[Serializable]
public struct SavedProcessingData
{
    public string productDataID;
    public DateTime completedTime; //TimeSpan => CompletedTime

    public SavedProcessingData(string productDataID, DateTime completedTime)
    {
        this.productDataID = productDataID;
        this.completedTime = completedTime;
    }
    
    
}
