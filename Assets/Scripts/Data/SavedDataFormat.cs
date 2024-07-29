using System;

using UnityEngine;

[Serializable]
public struct SavedFactoryData
{
    public string savedDataID;
    public string factoryDataID;
    public BoundsInt area;

    public SavedFactoryData(string savedDataID, string factoryDataID, BoundsInt area)
    {
        this.savedDataID = savedDataID;
        this.factoryDataID = factoryDataID;
        this.area = area;
    }
}

[Serializable]
public struct SavedProcessingData
{
    public string savedDataID;
    public string factoryDataID;
    public string productDataID;
    public DateTime completedTime;

    public SavedProcessingData(string savedDataID, string factoryDataID, string productDataID, DateTime completedTime)
    {
        this.savedDataID = savedDataID;
        this.factoryDataID = factoryDataID;
        this.productDataID = productDataID;
        this.completedTime = completedTime;
    }
}
