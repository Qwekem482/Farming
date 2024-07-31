using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class SaveData
{
    Dictionary<string, SavedFactoryData> factoryData;
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

    public void AddFactoryData(FactoryDataEvent info)
    {
        if (factoryData.ContainsKey(info.factoryID)) factoryData[info.factoryID] = info.CreateSavedFactoryData();
        else factoryData.Add(info.factoryID, info.CreateSavedFactoryData());
    }
}





