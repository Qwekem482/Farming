
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageSystem : SingletonMonoBehavior<StorageSystem>, IGameSystem
{
    readonly Dictionary<Collectible, int> allItems = new Dictionary<Collectible, int>();
    Dictionary<Collectible, int> existingItems = new Dictionary<Collectible, int>();
    
    [SerializeField] Collectible[] upgradeTools = new Collectible[3];
    
    int maxCapacity = 50;
    int currentCapacity;
    int level;
    
    #region MonoBehavior

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.AddListener<InsufficientCapacityEvent>(OnInsufficientCapacity);
        EventManager.Instance.AddListener<StorageItemChangeEvent>(OnStorageChange);
        
    }

    public void StartingSystem()
    {
        StorageUI.Instance.LoadStoringData(currentCapacity, maxCapacity, existingItems);
        StorageUI.Instance.LoadUpgradeData(Item.CreateArrayItem(upgradeTools, (level + 1)), maxCapacity);
    }
    

    public void LoadSavedData(int loadCapacity, int loadLevel)
    {
        //TODO: Load
        maxCapacity = loadCapacity > 50 ?
            loadCapacity :
            50;
        level = loadLevel;
        existingItems = GetExistingItem();
        foreach(int amount in existingItems.Values) currentCapacity += amount;
    }
    
    #endregion

    #region GameEventHandler

    void OnStorageChange(StorageItemChangeEvent info)
    {
        if (info.item.amount > 0)
        {
            IncreaseItem(info);
        } else
        {
            DecreaseItem(info);
        }
    }
    
    void OnInsufficientCapacity(InsufficientCapacityEvent info)
    {
        Debug.Log("Notify insufficient");
    }

    #endregion

    #region Logic

    bool IsSufficient(Item item)
    {
        return GetStoreAmount(item.collectible) >= item.amount;
    }

    public bool IsSufficient(IEnumerable<Item> items)
    {
        return items.All(item => GetStoreAmount(item.collectible) >= item.amount);
    }
    
    public int GetStoreAmount(Collectible collectible)
    {
        return allItems[collectible];
    }

    #endregion

    #region AlternativeMethods
    
    //info.item.amount > 0
    void IncreaseItem(StorageItemChangeEvent info)
    {
        if (info.item.amount + currentCapacity > maxCapacity)
        {
            EventManager.Instance.QueueEvent(new InsufficientCapacityEvent(info.item));
            return;
        } 
                
        EventManager.Instance.QueueEvent(new SufficientCapacityEvent(info.item));
        CalcRemainingItem(info);
    }

    //info.item.amount < 0
    void DecreaseItem(StorageItemChangeEvent info)
    {
        if (-info.item.amount > allItems[info.item.collectible])
        {
            EventManager.Instance.QueueEvent(new InsufficientItemsEvent(info.item));
            return;
        }
            
        EventManager.Instance.QueueEvent(new SufficientItemsEvent(info.item));
        CalcRemainingItem(info);
    }
    
    public void LoadItemFromDisk()
    {
        foreach(Collectible collectible in ResourceManager.Instance.allCollectibles)
        {
            allItems.Add(collectible,
                SaveLoadSystem.Instance.HasItem(collectible) ?
                    SaveLoadSystem.Instance.Amount(collectible) :
                    0);
        }
        
        foreach(KeyValuePair<Collectible, int> item in allItems) Debug.Log(item.Key + "|" + item.Value);
    }

    void CalcRemainingItem(StorageItemChangeEvent info)
    {
        allItems[info.item.collectible] += info.item.amount;
        currentCapacity += info.item.amount;
        existingItems = GetExistingItem();
        StorageUI.Instance.LoadStoringData(currentCapacity, maxCapacity, existingItems);
    }
    
    Dictionary<Collectible, int> GetExistingItem()
    {
        return allItems.Where(item => item.Value > 0)
            .ToDictionary(item => item.Key, item => item.Value);
    }
    
    #endregion

    #region Upgrade

    public void OnClickUpgrade()
    {
        int lackAmount = 0;
        foreach(Collectible tool in upgradeTools)
        {
            int storageToolAmount = GetStoreAmount(tool);
            if (storageToolAmount < level + 1) lackAmount += level + 1 - storageToolAmount;
        }

        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-lackAmount * 10, CurrencyType.Gold));
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientUpgradeGold);
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientUpgradeGold);
    }
    
    void IncreaseCapacity()
    {
        maxCapacity += 50;
        level++;
        StorageUI.Instance.LoadUpgradeData(Item.CreateArrayItem(upgradeTools, (level + 1)), maxCapacity);
        EventManager.Instance.QueueEvent(new SaveStorageCapacityEvent(maxCapacity, level));
    }

    void OnInsufficientUpgradeGold(InsufficientCurrencyEvent info)
    {
    }
    
    void OnSufficientUpgradeGold(SufficientCurrencyEvent info)
    {
        foreach(Collectible tool in upgradeTools)
        {
            int storageToolAmount = GetStoreAmount(tool);
            EventManager.Instance.QueueEvent(
                storageToolAmount < level + 1 ?
                    new StorageItemChangeEvent(new Item(tool, storageToolAmount)) :
                    new StorageItemChangeEvent(new Item(tool, level + 1)));
            
        }
        
        IncreaseCapacity();
    }

    #endregion
}


