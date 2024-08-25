using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    //public List<LevelData> levelData;
    public List<Collectible> allCollectibles;

    public readonly Dictionary<string, BuildingData> buildingData = new Dictionary<string, BuildingData>();
    public readonly Dictionary<string, ProductionOutputData> productData = new Dictionary<string, ProductionOutputData>();
    public SerializedDictionary<BuildingType, ShopItemData[]> shopItems;
    public Sprite silverSprite;
    public Sprite goldSprite;

    public void Initialization()
    {
        foreach(ShopItemData item in shopItems.Values.SelectMany(shopItem => shopItem))
        {
            buildingData.Add(item.building.id, item.building);

            if (item.building.GetType() == typeof(ProductionBuildingData))
            {
                foreach(ProductionOutputData data in ((ProductionBuildingData)item.building).productData) 
                    productData.Add(data.id, data);
            }
            
            
        }
    }

    [CanBeNull]
    public ProductionOutputData TranslateToProductData(string id)
    {
        return id == null ? null : productData[id];
    }
}

[Serializable]
public class LevelData
{
    public int levelCount;
    public int expNeeded;
    public int[] itemsUnlocked;
    public int[] currencyReward;
}
