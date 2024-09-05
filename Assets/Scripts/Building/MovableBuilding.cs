using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(PolygonCollider2D))]
public class MovableBuilding : MonoBehaviour
{
    public string uniqueID;
    public BuildingData buildingData;
    public BoundsInt buildingArea;
    public string buildingName;
    public bool IsPlaced { get; private set; }

    public virtual void Init(BuildingData data, BoundsInt area)
    {
        buildingData = data;
        buildingArea = area;
        buildingName = buildingData.buildingName;
    }

    public bool Placeable()
    {
        BoundsInt areaTemp = buildingArea;
        areaTemp.position = BuildingSystem.Instance.layout.LocalToCell(transform.position);
        areaTemp.position -= new Vector3Int(
            Mathf.CeilToInt((float)areaTemp.size.x / 2),
            Mathf.CeilToInt((float)areaTemp.size.y / 2),
            0);

        return BuildingSystem.Instance.ValidArea(areaTemp);
    }

    public virtual void Place()
    {
        BoundsInt areaTemp = buildingArea;
        areaTemp.position = BuildingSystem.Instance.layout.LocalToCell(transform.position);
        areaTemp.position -= new Vector3Int(
            Mathf.CeilToInt((float)areaTemp.size.x / 2),
            Mathf.CeilToInt((float)areaTemp.size.y / 2),
            0);

        IsPlaced = true;
        BuildingSystem.Instance.tempMap.SetTilesBlock(areaTemp,
            BuildingSystem.Instance.SetTileBaseArrayValue(areaTemp, TileType.White));
        
        CameraSystem.Instance.Unfollow();
    }

    protected virtual void OnMouseUp()
    {
    }

    protected void OnMouseDrag()
    {
        BuildingSystem.Instance.SetupBuild();
    }

}
