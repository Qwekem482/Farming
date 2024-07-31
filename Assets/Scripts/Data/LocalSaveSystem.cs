
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;


public class LocalSaveSystem
{
    readonly static string SAVE_FILE_LOCATION = Application.persistentDataPath + "/Saves";
    const string FILE_NAME = "save";
    const string EXTENSION = ".sav";
    
    public static string FileName { get; private set; }
    public static string FilePath { get; private set; }

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FILE_LOCATION))
            Directory.CreateDirectory(SAVE_FILE_LOCATION);

        FileName = FILE_NAME + EXTENSION;
        FilePath = SAVE_FILE_LOCATION + FileName;
    }

    public static bool Save<T>(T saveData)
    {
        try
        {
             if (File.Exists(FilePath)) File.Delete(FilePath);
             FileStream fileStream = File.Create(FilePath);
             fileStream.Close();
             File.WriteAllText(FilePath, JsonConvert.SerializeObject(saveData));
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
        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException(FilePath + " does not exist!");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError("Unable to load data because of: " + e.Message + "\n" + e.StackTrace);
            throw;
        }
    }
}
