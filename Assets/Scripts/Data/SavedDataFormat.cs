using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class SavedBuildingData
{
    public string buildingID;
    public string buildingDataID;
    public Vector3 position;
    public BoundsInt area;
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
