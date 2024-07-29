using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    //public List<LevelData> levelData;
    public List<BuildingData> allBuildingData;
    public List<Collectible> allCollectibles;
    
    [SerializedDictionary("ItemType", "List")] 
    public SerializedDictionary<ItemType, ShopItemData[]> shopItems;
    [SerializedDictionary("BuildingType", "List")] 
    public SerializedDictionary<FactoryType, ProductData[]> products;

}

[Serializable]
public class LevelData
{
    public int levelCount;
    public int expNeeded;
    public int[] itemsUnlocked;
    public int[] currencyReward;
}
