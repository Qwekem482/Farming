using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderSystem : SingletonMonoBehavior<OrderSystem>, IGameSystem
{
    [NonSerialized] public Order[] orders = new Order[9];


    readonly List<Collectible> availableCollectibles = new List<Collectible>();
    readonly OrderGenerator generator = new OrderGenerator();
    

    public void StartingSystem()
    {
        foreach(ProductionOutputData data in ResourceManager.Instance.productData.Values)
        {
            if(LevelSystem.Instance.currentLevel >= data.level) availableCollectibles.Add(data.product);
        }
        
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

    public void DeliveryOrder(int index)
    {
        if (!orders[index].CanBeDelivery()) return;
        
        foreach(Item item in orders[index].requestItems)
        {
            EventManager.Instance.QueueEvent(new StorageItemChangeEvent(item.ConvertToNegativeAmount()));
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
