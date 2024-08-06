using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    //public List<LevelData> levelData;
    public List<Collectible> allCollectibles;

    public Dictionary<string, BuildingData> allBuildingData = new Dictionary<string, BuildingData>();
    public Dictionary<string, ProductionOutputData> allProductData = new Dictionary<string, ProductionOutputData>();
    public SerializedDictionary<BuildingType, ShopItemData[]> shopItems;

    public void Initialization()
    {
        foreach(ShopItemData item in shopItems.Values.SelectMany(shopItem => shopItem))
        {
            allBuildingData.Add(item.building.id, item.building);

            if (item.building.GetType() == typeof(ProductionBuildingData))
            {
                foreach(ProductionOutputData data in ((ProductionBuildingData)item.building).productData) 
                    allProductData.Add(data.id, data);
            }
            
            
        }
    }

    public ProductData TranslateToProductData(string id)
    {
        return allProductData[id] as ProductData;
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
