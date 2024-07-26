using System;
using System.Collections;
using System.Collections.Generic;
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

    public void SavePlaceableData(string uniqueID, BoundsInt area)
    {
        SavedBuildingData data = new SavedBuildingData(area);
        if (placeableData.ContainsKey(uniqueID)) placeableData[uniqueID] = data;
        else placeableData.Add(uniqueID, data);
    }
    
    
}

public struct SavedBuildingData
{
    public BoundsInt area;

    public SavedBuildingData(BoundsInt area)
    {
        this.area = area;
    }
}


