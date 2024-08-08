using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Dictionary<string, SavedFactoryData> factoryData = new Dictionary<string, SavedFactoryData>();
    public Dictionary<string, SavedFieldData> fieldData = new Dictionary<string, SavedFieldData>();
    Dictionary<Collectible, int> storageData = new Dictionary<Collectible, int>();
    //public List<Quest> currentQuestList;

    string exp;
    string level;
    string gold;
    string silver;

    public static string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString();
    }

    public void AddFactoryData(SaveFactoryDataEvent info)
    {
        if (factoryData.ContainsKey(info.buildingID)) factoryData[info.buildingID] = info.CreateSavedFactoryData();
        else factoryData.Add(info.buildingID, info.CreateSavedFactoryData());
    }
    
    public void AddFieldData(SaveFieldDataEvent info)
    {
        if (fieldData.ContainsKey(info.buildingID)) fieldData[info.buildingID] = info.CreateSavedFieldData();
        else fieldData.Add(info.buildingID, info.CreateSavedFieldData());
    }

    public void ModifyStorageData()
    {
        
    }

    public override string ToString()
    {
        return factoryData.Aggregate("",
            (current, data)
                => current + (data.Key + " : " + data.Value));
    }
}





