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
    public Dictionary<string, ProductData> allProductData = new Dictionary<string, ProductData>();
    public SerializedDictionary<BuildingType, ShopItemData[]> shopItems;

    public void Initialization()
    {
        foreach(ShopItemData item in shopItems.Values.SelectMany(shopItem => shopItem))
        {
            allBuildingData.Add(item.building.id, item.building);

            if (item.building.GetType() == typeof(FactoryData))
            {
                foreach(ProductData data in ((FactoryData)item.building).productData) 
                    allProductData.Add(data.id, data);
            }
            
            
        }
    }

    public ProductData TranslateToProductData(string id)
    {
        return allProductData[id];
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
