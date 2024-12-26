using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class LocalSaveSystem
{
    readonly static string SAVE_FILE_LOCATION = Application.persistentDataPath + "/Saves/";
    const string FILE_NAME = "save";
    const string EXTENSION = ".sav";

    static string FileName
    {
        get
        {
            return FILE_NAME + EXTENSION;
        }
    }
    static string FilePath
    {
        get
        {
            return SAVE_FILE_LOCATION + FileName;
        }
    }

    static void Init()
    {
        if (!Directory.Exists(SAVE_FILE_LOCATION))
        {
            Directory.CreateDirectory(SAVE_FILE_LOCATION);
        }
        
        FileStream fileStream = File.Create(FilePath);
        fileStream.Close();
    }

    public static bool Save<T>(T saveData)
    {
        Init();
        try
        {
             if (File.Exists(FilePath)) File.Delete(FilePath);
             FileStream fileStream = File.Create(FilePath);
             fileStream.Close();
             File.WriteAllText(FilePath, JsonConvert.SerializeObject(saveData, Formatting.Indented, new JsonSerializerSettings
             {
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
             }));
             Debug.Log("Write Save: " + FilePath);
             return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Unable to save data because of: " + e.Message + "\n" + e.StackTrace);
            return false;
        }
    }

    public static T LoadData<T>()
    {
        if (!File.Exists(FilePath)) Init();
        try
        {
            string content = !string.IsNullOrEmpty(File.ReadAllText(FilePath)) ?
                File.ReadAllText(FilePath) :
                "{\n  \"storageCapacity\": 50,\n  \"storageLevel\": 1,\n  \"exp\": 0,\n  \"level\": 1,\n  \"gold\": 999,\n  \"silver\": 9999,\n}";
            
            T data = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Unable to load data because of: " + e.Message);
            throw;
        }
    }
}
