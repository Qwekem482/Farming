using System;
using System.Collections.Generic;

public class SaveData
{
    public Dictionary<string, SavedFactoryData> factoryData = new Dictionary<string, SavedFactoryData>();
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

    public void AddFactoryData(FactoryDataEvent info)
    {
        if (factoryData.ContainsKey(info.factoryID)) factoryData[info.factoryID] = info.CreateSavedFactoryData();
        else factoryData.Add(info.factoryID, info.CreateSavedFactoryData());
    }

    public void ModifyStorageData()
    {
        
    }
}





