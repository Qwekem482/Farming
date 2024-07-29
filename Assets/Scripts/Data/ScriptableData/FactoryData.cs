using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactoryData", menuName = "CustomObject/FactoryData")]
public class FactoryData : BuildingData
{
    public FactoryType type;
    
    void OnValidate()
    {
        area.size = new Vector3Int(area.size.x, area.size.y, 1);
        if (char.IsDigit(id[0])) id = "F" + id;
    }
}
public enum FactoryType
{
    Grinder,
    SteamStation,
    Field,
}
