using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactoryData", menuName = "CustomObject/FactoryData")]
public class FactoryData : BuildingData
{
    public FactoryType type;
}
public enum FactoryType
{
    Grinder,
    SteamStation,
    Field,
}
