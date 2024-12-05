using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class MovableBuilding : MonoBehaviour
{
    public string uniqueID;
    public BuildingData buildingData;
    public BoundsInt buildingArea;
    public string buildingName;
    protected bool isPlaced;
    
    public virtual bool IsPlaced {
        get
        {
            return isPlaced;
        }
        protected set
        {
            isPlaced = value;
        }
    }
    
    bool isMouseDown;
    float mouseDownTime;

    public virtual void Init(BuildingData data, BoundsInt area, string id = default)
    {
        buildingData = data;
        buildingArea = area;
        buildingName = buildingData.buildingName;
        if (id == default) uniqueID = SaveData.GenerateUniqueID();
        else uniqueID = id;
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
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        BuildingSystem.Instance.tempMap.SetTilesBlock(areaTemp,
            BuildingSystem.Instance.SetTileBaseArrayValue(areaTemp, TileType.White));
        
        CameraSystem.Instance.Unfollow();
    }

    protected virtual void OnMouseUp()
    {
        isMouseDown = false;
        mouseDownTime = 0;
        AudioManager.Instance.PlayEffectClip(0);
    }

    protected virtual void OnMouseDown()
    {
        if (UICurtain.Instance.gameObject.activeSelf) return;
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
        IsPlaced = false;
        BuildingSystem.Instance.MoveBuilding(this);
    }

}
