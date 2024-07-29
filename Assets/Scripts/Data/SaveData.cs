using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class SaveData
{
    Dictionary<string, SavedFactoryData> placeableData;
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

    /*public void AddPlaceableData(string uniqueID, BoundsInt area)
    {
        SavedFactoryData data = new SavedFactoryData(uniqueID, area);
        if (placeableData.ContainsKey(uniqueID)) placeableData[uniqueID] = data;
        else placeableData.Add(uniqueID, data);
    }

    public void RemovePlaceableData(string uniqueID)
    {
        if (placeableData.ContainsKey(uniqueID)) placeableData.Remove(uniqueID);
    }*/
    
}





