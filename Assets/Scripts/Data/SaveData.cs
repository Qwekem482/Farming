using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
    //public List<Quest> currentQuestList;
    public int storageCapacity;
    public int storageLevel;
    public string exp;
    public string level;
    public int gold;
    public int silver;

    public Dictionary<string, int> storageData = new Dictionary<string, int>();
    public Dictionary<string, SavedFactoryData> factoryData = new Dictionary<string, SavedFactoryData>();
    public Dictionary<string, SavedFieldData> fieldData = new Dictionary<string, SavedFieldData>();
    public static string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString();
    }

    public void AddFactoryData(SaveFactoryDataEvent info)
    {
        if (factoryData.ContainsKey(info.buildingID)) 
            factoryData[info.buildingID] = info.CreateSavedFactoryData();
        else factoryData.Add(info.buildingID, info.CreateSavedFactoryData());
    }
    
    public void AddFieldData(SaveFieldDataEvent info)
    {
        if (fieldData.ContainsKey(info.buildingID)) 
            fieldData[info.buildingID] = info.CreateSavedFieldData();
        else fieldData.Add(info.buildingID, info.CreateSavedFieldData());
    }

    public void ModifyStorageData(SufficientCapacityEvent info)
    {
        if (storageData.ContainsKey(info.item.collectible.id)) 
            storageData[info.item.collectible.id] += info.item.amount;
        else storageData.Add(info.item.collectible.id, info.item.amount);
    }
    
    public void ModifyStorageData(SufficientItemsEvent info)
    {
        if (storageData.ContainsKey(info.item.collectible.id)) 
            storageData[info.item.collectible.id] += info.item.amount;
        else storageData.Add(info.item.collectible.id, info.item.amount);
    }

    public void UpdateStorageCapacity(SaveStorageCapacityEvent info)
    {
        storageCapacity = info.capacity;
        storageLevel = info.level;
    }

    public void UpdateCurrency(SaveCurrencyEvent info)
    {
        gold = info.gold;
        silver = info.silver;
    }
    

    public override string ToString()
    {
        return factoryData.Aggregate("",
            (current, data)
                => current + (data.Key + " : " + data.Value));
    }
}





