using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingSystem : SingletonMonoBehavior<BuildingSystem>, IGameSystem
{
    public GridLayout layout;
    [SerializeField]public Tilemap tempMap;
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
        construction.Init(data, data.area);
        tempBuilding = construction;
        
        TileFollowBuilding();
        
        CameraSystem.Instance.SetupFollow(tempBuilding.transform);
    }

    void ClearArea()
    {
        prevBase ??= SetTileBaseArrayValue(prevArea, TileType.Empty);
        tempMap.SetTilesBlock(prevArea, prevBase);
    }

    void TileFollowBuilding()
    {
        ClearArea();

        tempBuilding.buildingArea.position =
            layout.WorldToCell(tempBuilding.gameObject.transform.position) -
            new Vector3Int(
                Mathf.CeilToInt((float)tempBuilding.buildingArea.size.x / 2),
                Mathf.CeilToInt((float)tempBuilding.buildingArea.size.y / 2),
                0);
        
        TileBase[] baseArray = tempMap.GetTilesBlock(tempBuilding.buildingArea);
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
        
        prevBase = tempMap.GetTilesBlock(tempBuilding.buildingArea);
        tempMap.SetTilesBlock(tempBuilding.buildingArea, tileArray);
        prevArea = tempBuilding.buildingArea;
    }

    public void ColorTileFollowBuilding(BoundsInt area)
    {
        TileBase[] baseArray = tempMap.GetTilesBlock(area);
        TileBase[] tileArray = new TileBase[baseArray.Length];

        for (int i = 0; i < baseArray.Length; i++)
        {
            tileArray[i] = tileBases[TileType.White];
        }
        
        tempMap.SetTilesBlock(area, tileArray);
    }

    public bool ValidArea(BoundsInt area)
    {
        TileBase[] baseArray = tempMap.GetTilesBlock(area);
        return baseArray.All(tile => tile == tileBases[TileType.Green]);

    }

    public TileBase[] SetTileBaseArrayValue(BoundsInt area, TileType type)
    {
        TileBase[] toReturn = new TileBase[area.size.x * area.size.y * area.size.z];
        FillTiles(toReturn, type);
        Debug.Log("Type: " + type + "|Area: " + area);
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
