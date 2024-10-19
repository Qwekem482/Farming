using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class SavedBuildingData
{
    public string buildingID;
    public string buildingDataID;
    public Vector3 position;
    public BoundsInt area;

    public SavedBuildingData()
    {
    }
    
    public SavedBuildingData(string buildingID, string buildingDataID, Vector3 position, BoundsInt area)
    {
        this.buildingID = buildingID;
        this.buildingDataID = buildingDataID;
        this.position = position;
        this.area = area;
    }
}

[Serializable]
public class SavedFactoryData : SavedBuildingData
{
    public int queueCapacity;
    public Queue<SavedProcessingData> processing; 
    public Queue<string> completed; //productDataID of completed item

    public SavedFactoryData(string buildingID, string buildingDataID, Vector3 position, 
        BoundsInt area, int queueCapacity, Queue<SavedProcessingData> processing, 
        Queue<string> completed)
    {
        base.buildingID = buildingID;
        base.buildingDataID = buildingDataID;
        base.position = position;
        base.area = area;
        this.queueCapacity = queueCapacity;
        this.processing = processing;
        this.completed = completed;
    }

    public override string ToString()
    {
        string toReturn = "";

        toReturn = "buildingID : " + buildingID + "\n" +
                   "buildingDataID : " + buildingDataID + "\n" +
                   "position : " + position + "\n" +
                   "area : " + area + "\n" +
                   "queueCapacity : " + queueCapacity + "\n" +
                   "processing: \n";

        toReturn = processing.Aggregate(toReturn, (current, data) => current + (data + "\n"));

        toReturn += "completed : \n";

        return completed.Aggregate(toReturn, (current, data) => current + (data + "\n"));
    }
}

[Serializable]
public class SavedFieldData : SavedBuildingData
{
    public SavedProcessingData processingData;
    
    public SavedFieldData(string buildingID, string buildingDataID, Vector3 position, 
        BoundsInt area, SavedProcessingData processingData)
    {
        base.buildingID = buildingID;
        base.buildingDataID = buildingDataID;
        base.position = position;
        base.area = area;
        this.processingData = processingData;
    }
}

[Serializable]
public class SavedProcessingData
{
    public string productDataID;
    public DateTime completedDateTime;

    public SavedProcessingData(string productDataID, DateTime completedDateTime)
    {
        this.productDataID = productDataID;
        this.completedDateTime = completedDateTime;
    }

    public override string ToString()
    {
        string toReturn = "productDataID : " + productDataID + "\n" +
                          "completedTime : " + completedDateTime.ToString("F");
        return toReturn;
    }
}

[Serializable]
public class SavedOrderData
{
    public readonly int index;
    public readonly Dictionary<string, int> requestItems = new Dictionary<string, int>();
    public readonly int expReward = 0;
    public readonly int silverReward = 0;
    public readonly int goldReward = 0;
    public readonly DateTime resetTime;
    
    public SavedOrderData(int index, Order order)
    {
        this.index = index;
        expReward = order.exp;
        silverReward = order.silver;
        goldReward = order.gold;
        resetTime = order.resetTime;

        foreach(Item item in order.requestItems)
        {
            requestItems.Add(item.collectible.id, item.amount);
        }
    }

    [JsonConstructor]
    public SavedOrderData(int index, Dictionary<string, int> requestItems,
        int expReward, int silverReward, int goldReward, DateTime resetTime)
    {
        this.index = index;
        this.requestItems = requestItems;
        this.expReward = expReward;
        this.silverReward = silverReward;
        this.goldReward = goldReward;
        this.resetTime = resetTime;
    }
}