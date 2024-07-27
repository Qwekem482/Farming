using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSaveSystem
{
    readonly static string SAVE_FILE_LOCATION = Application.persistentDataPath + "/Saves";
    public const string FILE_NAME = "save";
    const string EXTENSION = ".sav";
    
    public static string FileName { get; private set; }

}
