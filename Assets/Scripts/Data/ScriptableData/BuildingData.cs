using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingData : ScriptableObject
{
    public BuildingType type;
    public Sprite sprite;
    public BoundsInt area;
    public TimePeriod constructionTime;
}

public enum BuildingType
{
    Grinder,
    SteamStation,
    Field,
}
