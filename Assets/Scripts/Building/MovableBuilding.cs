using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool IsPlaced { get; protected set; }
    
    bool isMouseDown;
    float mouseDownTime;

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
            Mathf.CeilToInt((float)areaTemp.size.x),
            Mathf.CeilToInt((float)areaTemp.size.y),
            0);

        return BuildingSystem.Instance.ValidArea(areaTemp);
    }

    public virtual void Place()
    {
        BoundsInt areaTemp = buildingArea;
        areaTemp.position = BuildingSystem.Instance.layout.LocalToCell(transform.position);
        areaTemp.position -= new Vector3Int(
            Mathf.CeilToInt((float)areaTemp.size.x),
            Mathf.CeilToInt((float)areaTemp.size.y),
            0);

        IsPlaced = true;
        BuildingSystem.Instance.tempMap.SetTilesBlock(areaTemp,
            BuildingSystem.Instance.SetTileBaseArrayValue(areaTemp, TileType.White));
        
        CameraSystem.Instance.Unfollow();
    }

    protected virtual void OnMouseUp()
    {
        isMouseDown = false;
        mouseDownTime = 0;
    }

    void OnMouseDown()
    {
        isMouseDown = true;
        mouseDownTime = Time.time;
    }

    protected void OnMouseDrag()
    {
        if (GetType() != typeof(BuildableBuilding) && isMouseDown && Time.time - mouseDownTime >= 2)
        {
            MoveBuilding();
            isMouseDown = false;
        }
        
        BuildingSystem.Instance.SetupBuild();
    }

    protected virtual void SaveState()
    {
    }

    void MoveBuilding()
    {
        Debug.Log("MoveBuilding");
        IsPlaced = false;
        BuildingSystem.Instance.MoveBuilding(this);
    }

}
