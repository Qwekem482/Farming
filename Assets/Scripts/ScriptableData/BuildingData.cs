using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BuildingData", menuName = "CustomObject/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string id;
    public string buildingName;
    public Sprite sprite;
    public BoundsInt area;
    public TimePeriod constructionTime;
    public BuildingType buildingType;
    
    protected virtual void OnValidate()
    {
        area.size = new Vector3Int(area.size.x, area.size.y, 1);
        if (char.IsDigit(id[0])) id = "B" + id;
    }
}
