using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class SaveData
{
    Dictionary<string, SavedBuildingData> placeableData;
    Dictionary<Collectible, int> storageData;
    Dictionary<string, Queue> productionQueueData;
    //public List<Quest> currentQuestList;

    string exp;
    string level;
    string gold;
    string silver;

    public static string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString();
    }

    public void AddPlaceableData(string uniqueID, BoundsInt area)
    {
        SavedBuildingData data = new SavedBuildingData(uniqueID, area);
        if (placeableData.ContainsKey(uniqueID)) placeableData[uniqueID] = data;
        else placeableData.Add(uniqueID, data);
    }

    public void RemovePlaceableData(string uniqueID)
    {
        if (placeableData.ContainsKey(uniqueID)) placeableData.Remove(uniqueID);
    }

    [OnDeserialized]
    internal void OnDeserialize(StreamingContext context)
    {
        placeableData ??= new Dictionary<string, SavedBuildingData>();
    }
    
}

[Serializable]
public struct SavedBuildingData
{
    public string id;
    public BoundsInt area;

    public SavedBuildingData(string id, BoundsInt area)
    {
        this.id = id;
        this.area = area;
    }
}


