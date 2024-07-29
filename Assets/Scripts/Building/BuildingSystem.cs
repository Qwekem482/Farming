using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class BuildingSystem : SingletonMonoBehavior<BuildingSystem>, IGameSystem
{
    public GridLayout layout;
    [SerializeField] Tilemap mainMap;
    [SerializeField] Tilemap tempMap;
    [SerializeField] Camera mainCam;

    [SerializedDictionary("Type", "Tile Sample")]
    [SerializeField] SerializedDictionary<TileType, TileBase> tileBases;
    
    public Material grayscale;
    [SerializeField] Transform buildingParent;
    
    MovableBuilding tempBuilding;
    Vector3Int prevPosition;
    BoundsInt prevArea;
    TileBase[] prevBase;

    
    #region MonoBehavior

    public void StartingSystem()
    {
        CloseBuildingMode();
    }

    #endregion

    #region Controller

    public void SetupBuild(/*MovableBuilding building*/)
    {
        if (!tempBuilding) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (tempBuilding.IsPlaced) return;

        Vector2 touchPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = layout.LocalToCell(touchPosition);

        if (prevPosition == cellPosition) return;

        tempBuilding.transform.localPosition = layout.CellToLocalInterpolated
            (cellPosition);
        prevPosition = cellPosition;
        TileFollowBuilding();
    }

    public void CancelBuild()
    {
        ClearArea();
        Destroy(tempBuilding.gameObject);
        CloseBuildingMode();
    }

    public void ConfirmBuild()
    {
        if (!tempBuilding.Placeable()) return;
        
        tempBuilding.Place();
        
        tempBuilding = null;
        prevArea = new BoundsInt();
        prevPosition = new Vector3Int();
        prevBase = null;
        
        CloseBuildingMode();
    }

    void OpenBuildingMode()
    {
        HorizontalUIHolder.Instance.OpenUI(false);
        tempMap.gameObject.SetActive(true);
    }

    void CloseBuildingMode()
    {
        HorizontalUIHolder.Instance.CloseUI();
        tempMap.gameObject.SetActive(false);
    }

    #endregion

    #region Placement

    public void InstantiateConstruction(BuildingData data)
    {
        OpenBuildingMode();

        GameObject emptyBuilding = new GameObject
        {
            transform =
            {
                parent = buildingParent,
            },
            name = data.buildingName,
        };

        SpriteRenderer sRenderer = emptyBuilding.AddComponent<SpriteRenderer>();
        sRenderer.sprite = data.sprite;
        sRenderer.material = grayscale;
        sRenderer.color = new Color(255, 255, 255, 128);
        
        BuildableBuilding construction = emptyBuilding.AddComponent<BuildableBuilding>();
        construction.Init(data);
        tempBuilding = construction;
        
        TileFollowBuilding();
        
        PanZoom.Instance.SetupFollow(tempBuilding.transform);
    }

    void ClearArea()
    {
        prevBase ??= SetTileBaseArrayValue(prevArea, TileType.Empty);
        tempMap.SetTilesBlock(prevArea, prevBase);
    }

    void TileFollowBuilding()
    {
        ClearArea();

        tempBuilding.area.position =
            layout.WorldToCell(tempBuilding.gameObject.transform.position) -
            new Vector3Int(
                Mathf.CeilToInt((float)tempBuilding.area.size.x / 2),
                Mathf.CeilToInt((float)tempBuilding.area.size.y / 2),
                0);
        
        TileBase[] baseArray = tempMap.GetTilesBlock(tempBuilding.area);
        TileBase[] tileArray = new TileBase[baseArray.Length];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.Empty])
            {
                tileArray[i] = tileBases[TileType.Green];

            } else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        
        prevBase = tempMap.GetTilesBlock(tempBuilding.area);
        tempMap.SetTilesBlock(tempBuilding.area, tileArray);
        prevArea = tempBuilding.area;
    }

    public bool ValidArea(BoundsInt area)
    {
        TileBase[] baseArray = tempMap.GetTilesBlock(area);
        foreach(var tile in baseArray)
        {
            if (tile != tileBases[TileType.Green])
            {
                return false;
            }
        }

        return true;
    }

    public TileBase[] SetTileBaseArrayValue(BoundsInt area, TileType type)
    {
        TileBase[] toReturn = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toReturn, type);

        return toReturn;
    }

    #endregion

    #region Tilemap

    void FillTiles(IList<TileBase> tiles, TileType type)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i] = tileBases[type];
        }
    }

    #endregion
    
    
}

public enum TileType
{
    Empty,
    White,
    Red,
    Green,
}
