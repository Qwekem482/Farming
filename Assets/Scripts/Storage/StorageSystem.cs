
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageSystem : SingletonMonoBehavior<StorageSystem>, IGameSystem
{
    readonly Dictionary<Collectible, int> allItems = new Dictionary<Collectible, int>();
    Dictionary<Collectible, int> existingItems = new Dictionary<Collectible, int>();
    
    [SerializeField] Collectible[] upgradeTools = new Collectible[3];
    
    int maxCapacity = 10;
    int currentCapacity;
    int level;
    
    #region MonoBehavior

    protected override void Awake()
    {
        base.Awake();
        LoadItemFromDisk();
        existingItems = GetExistingItem();
    }

    public void StartingSystem()
    {
        LoadItemFromDisk();
        
        EventManager.Instance.AddListener<InsufficientCapacityEvent>(OnInsufficientCapacity);
        EventManager.Instance.AddListener<StorageItemChangeEvent>(OnStorageChange);
        
        StorageUI.Instance.LoadStoringData(currentCapacity, maxCapacity, existingItems);
        StorageUI.Instance.LoadUpgradeData(Item.CreateArrayItem(upgradeTools, (level + 1)), maxCapacity);
    }

    public void LoadSavedData()
    {
        //TODO: Load
        
        existingItems = GetExistingItem();
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
    
    public void IncreaseCapacity()
    {
        if (upgradeTools.Any(tool => !IsSufficient(new Item(tool, level + 1))))
        {
            Debug.Log("Insufficient");
            return;
        }
        
        maxCapacity += 50;

        foreach(Collectible tool in upgradeTools)
        {
            EventManager.Instance.QueueEvent(new StorageItemChangeEvent(new Item(tool, (level + 1))));
        }
        
        level++;
        
        StorageUI.Instance.LoadUpgradeData(Item.CreateArrayItem(upgradeTools, (level + 1)), maxCapacity);
    }

    bool IsSufficient(Item item)
    {
        return GetCollectibleStoreAmount(item.collectible) >= item.amount;
    }

    public bool IsSufficient(IEnumerable<Item> items)
    {
        return items.All(item => GetCollectibleStoreAmount(item.collectible) >= item.amount);
    }
    
    public int GetCollectibleStoreAmount(Collectible collectible)
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
    
    void LoadItemFromDisk()
    {
        foreach(Collectible collectible in ResourceManager.Instance.allCollectibles)
        {
            allItems.Add(collectible, 0);
        }
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
    
}


