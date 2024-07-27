using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

public class ResourceManager : SingletonMonoBehavior<ResourceManager>
{
    //public List<LevelData> levelData;
    public List<BuildingData> allBuildingData;
    [SerializedDictionary("Type", "List")] 
    public SerializedDictionary<ItemType, List<ShopItem>> shopItems;
    public List<Collectible> allCollectibles;
}

[Serializable]
public class LevelData
{
    public int levelCount;
    public int expNeeded;
    public int[] itemsUnlocked;
    public int[] currencyReward;
}
