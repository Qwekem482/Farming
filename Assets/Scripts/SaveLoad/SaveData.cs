using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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
    public Dictionary<string, SavedProcessBuildingData> processBuilding = new Dictionary<string, SavedProcessBuildingData>();
    public Dictionary<string, SavedFactoryData> factoriesData = new Dictionary<string, SavedFactoryData>();
    public Dictionary<string, SavedFieldData> fieldsData = new Dictionary<string, SavedFieldData>();
    public Dictionary<string, SavedBuildingData> decorsData = new Dictionary<string, SavedBuildingData>();
    public SavedOrderData[] ordersData = new SavedOrderData[9];
    public static string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString();
    }

    public void AddProcessingBuildingData(SaveProcessBuildingEvent info)
    {
        if (processBuilding.ContainsKey(info.buildingID)) 
            processBuilding[info.buildingID] = info.CreateSavedProcessBuildingData();
        else processBuilding.Add(info.buildingID, info.CreateSavedProcessBuildingData());
    }

    public void RemoveProcessBuildingData(RemoveSaveProcessBuildingEvent info)
    {
        processBuilding.Remove(info.buildingID);
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

    public void AddDecorationData(SaveBuildingDataEvent info)
    {
        if (decorsData.ContainsKey(info.buildingID))
            decorsData[info.buildingID] = info.CreateSavedBuildingData();
        else decorsData.Add(info.buildingID, info.CreateSavedBuildingData());
    }

    public void ModifyStorageCapacityData(SufficientCapacityEvent info)
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
    
    public void UpdateExp(SaveExpEvent info)
    {
        exp = info.currentExp;
        level = info.level;
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





