using System;
using System.Collections.Generic;
using System.Linq;
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

    public override string ToString()
    {
        string toReturn = "";

        toReturn = "factoryID : " + factoryID + "\n" +
                   "factoryDataID : " + factoryDataID + "\n" +
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
public class SavedProcessingData
{
    public string productDataID;
    public DateTime completedTime; //TimeSpan => CompletedTime

    public SavedProcessingData(string productDataID, DateTime completedTime)
    {
        this.productDataID = productDataID;
        this.completedTime = completedTime;
    }

    public override string ToString()
    {
        string toReturn = "productDataID : " + productDataID + "\n" +
                          "completedTime : " + completedTime.ToString("F");
        return toReturn;
    }
}
