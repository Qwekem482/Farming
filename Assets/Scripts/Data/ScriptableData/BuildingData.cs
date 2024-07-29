using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BuildingData", menuName = "CustomObject/BuildingData")]
public abstract class BuildingData : ScriptableObject
{
    public string id;
    public string buildingName;
    public Sprite sprite;
    public BoundsInt area;
    public TimePeriod constructionTime;
    public BuildingType buildingType;
}
