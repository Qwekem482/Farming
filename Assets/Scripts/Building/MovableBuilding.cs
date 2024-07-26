using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PolygonCollider2D))]
public class MovableBuilding : MonoBehaviour
{
    public string uniqueID;
    public BoundsInt area;
    protected BuildingType type;
    
    public bool IsPlaced { get; private set; }

    public virtual void Init(BuildingData data)
    {
        area = data.area;
        type = data.type;
    }

    public bool Placeable()
    {
        BoundsInt areaTemp = area;
        areaTemp.position = BuildingSystem.Instance.layout.LocalToCell(transform.position);
        areaTemp.position -= new Vector3Int(
            Mathf.CeilToInt((float)areaTemp.size.x / 2),
            Mathf.CeilToInt((float)areaTemp.size.y / 2),
            0);

        return BuildingSystem.Instance.ValidArea(areaTemp);
    }

    public virtual void Place()
    {
        BoundsInt areaTemp = area;
        areaTemp.position = BuildingSystem.Instance.layout.LocalToCell(transform.position);
        areaTemp.position -= new Vector3Int(
            Mathf.CeilToInt((float)areaTemp.size.x / 2),
            Mathf.CeilToInt((float)areaTemp.size.y / 2),
            0);

        IsPlaced = true;
        BuildingSystem.Instance.SetTileBaseArrayValue(areaTemp, TileType.White);
        
        PanZoom.Instance.Unfollow();
    }

    protected virtual void OnMouseUp()
    {
    }
    
}
