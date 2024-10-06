using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int storageCapacity;
    public int storageLevel;
    public int exp;
    public int level;
    public int gold;
    public int silver;

    public Dictionary<string, int> storageData = new Dictionary<string, int>();
    public Dictionary<string, SavedFactoryData> factoriesData = new Dictionary<string, SavedFactoryData>();
    public Dictionary<string, SavedFieldData> fieldsData = new Dictionary<string, SavedFieldData>();
    public SavedOrderData[] ordersData = new SavedOrderData[9];
    public static string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString();
    }

    public void AddFactoryData(SaveFactoryDataEvent info)
    {
        if (factoriesData.ContainsKey(info.buildingID)) 
            factoriesData[info.buildingID] = info.CreateSavedFactoryData();
        else factoriesData.Add(info.buildingID, info.CreateSavedFactoryData());
    }
    
    public void AddFieldData(SaveFieldDataEvent info)
    {
        if (fieldsData.ContainsKey(info.buildingID)) 
            fieldsData[info.buildingID] = info.CreateSavedFieldData();
        else fieldsData.Add(info.buildingID, info.CreateSavedFieldData());
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

    public void UpdateOrder(SaveOrderEvent info)
    {
        ordersData[info.index] = new SavedOrderData(info.index, info.order);
    }

    public override string ToString()
    {
        return factoriesData.Aggregate("",
            (current, data)
                => current + (data.Key + " : " + data.Value));
    }
}





