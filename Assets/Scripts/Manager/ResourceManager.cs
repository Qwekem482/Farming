using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using UnityEngine;

public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    //public List<LevelData> levelData;
    public SerializedDictionary<string, Collectible> collectiblesData;
    public SerializedDictionary<string, BuildingData> buildingData;
    public SerializedDictionary<string, ProductionOutputData> productData;
    public SerializedDictionary<BuildingType, ShopBuildingData[]> shopItems;
    public List<LevelData> levelData;
    public Sprite silverSprite;
    public Sprite goldSprite;
    public Material grayscale;

    [CanBeNull]
    public ProductionOutputData TranslateToProductData(string id)
    {
        return id == null ? null : productData[id];
    }
    
#if UNITY_EDITOR

    [SerializeField] List<Collectible> addCollectibles;
    
    void OnValidate()
    {
        AddBuildingFromShopItem();
        ValidateLevel();
        ValidateCollectibles();
    }

    void ValidateLevel()
    {
        for (int i = 0; i < levelData.Count; i++)
        {
            levelData[i] ??= new LevelData();
            levelData[i].level = i + 1;
            levelData[i].expNeeded = (i + 1) * 10;
            levelData[i].silver = (i + 1) * 50;
            levelData[i].gold = (int) Mathf.Clamp((int)((i + 1) / 10), 1, Mathf.Infinity);
        }
    }

    void AddBuildingFromShopItem()
    {
        foreach(ShopBuildingData item in shopItems.Values.SelectMany(shopItem => shopItem))
        {
            buildingData.TryAdd(item.building.id, item.building);

            if (item.building.GetType() == typeof(ProductionBuildingData))
            {
                foreach(ProductionOutputData data in ((ProductionBuildingData)item.building).productData) 
                    productData.TryAdd(data.productID, data);
            }
            
        }
    }

    void ValidateCollectibles()
    {
        foreach(Collectible collectible in addCollectibles)
        {
            if(collectible == null || collectiblesData.ContainsKey(collectible.id)) continue;
            collectiblesData.Add(collectible.id, collectible);
        }
        
        addCollectibles.Clear();
    }
    
#endif
}
