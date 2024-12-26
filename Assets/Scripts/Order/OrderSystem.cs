using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderSystem : SingletonMonoBehavior<OrderSystem>, IGameSystem
{
    [NonSerialized] public Order[] orders = new Order[9];
    readonly List<Collectible> availableCollectibles = new List<Collectible>();
    readonly OrderGenerator generator = new OrderGenerator();
    const float TOOL_ORDER_RATE = 0.01f;

    public void StartingSystem()
    {
        SetAvailableCollectible();
        EventManager.Instance.AddListener<LevelUpEvent>(_ =>
        {
            SetAvailableCollectible();
        });
        
        for (int i = 0; i < 9; i++)
        {
            orders[i] ??= generator.Generate(availableCollectibles);
            Save(i);
        }
    }
    
    void Save(int index)
    {
        EventManager.Instance.QueueEvent(new SaveOrderEvent(index, orders[index]));
    }
    
    public void Load(Order[] orderArray)
    {
        orders = orderArray.Length == 9 ? orderArray : new Order[9];
    }

    public void CancelOrder(int index)
    {
        CreateNewOrder(index, true);
        Debug.Log("CompletedCancel");
    }

    void SetAvailableCollectible()
    {
        foreach(ProductionOutputData data in ResourceManager.Instance.productData.Values)
        {
            if(LevelSystem.Instance.currentLevel >= data.level) availableCollectibles.Add(data.product);
        }
    }

    public void DeliveryOrder(int index)
    {
        if (!orders[index].CanBeDelivery()) return;
        
        foreach(Item item in orders[index].requestItems)
        {
            EventManager.Instance.QueueEvent(
                new StorageItemChangeEvent(item.ConvertToNegativeAmount()));
        }

        if (Random.Range(0f, 1f) < TOOL_ORDER_RATE)
        {
            EventManager.Instance.QueueEvent(
                new StorageItemChangeEvent(
                    new Item(StorageSystem.Instance.upgradeTools[Random.Range(0, 3)], 1)));
        }

        EventManager.Instance.QueueEvent(
            orders[index].gold == 0
                ? new CurrencyChangeEvent(orders[index].silver, CurrencyType.Silver)
                : new CurrencyChangeEvent(orders[index].gold, CurrencyType.Gold));
        EventManager.Instance.QueueEvent(new ExpAddedEvent(orders[index].exp));

        CreateNewOrder(index, false);
    }

    void CreateNewOrder(int index, bool isCooldown)
    {
        orders[index] = generator.Generate(availableCollectibles, isCooldown);
        Save(index);
    }
    
    
}
