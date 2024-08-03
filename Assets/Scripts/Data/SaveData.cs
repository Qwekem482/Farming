using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
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
        Debug.Log("Add Factory Data: " + info);
        if (factoryData.ContainsKey(info.factoryID)) factoryData[info.factoryID] = info.CreateSavedFactoryData();
        else factoryData.Add(info.factoryID, info.CreateSavedFactoryData());
    }

    public void ModifyStorageData()
    {
        
    }

    public override string ToString()
    {
        string toReturn = "";
        foreach(KeyValuePair<string, SavedFactoryData> data in factoryData)
        {
            toReturn += data.Key + " : " + data.Value;
        }

        return toReturn;
    }
}





