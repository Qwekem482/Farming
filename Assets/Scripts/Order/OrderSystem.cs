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
    }

    public void DeliveryOrder(int index)
    {
        if (!orders[index].CanBeDelivery()) return;
        CreateNewOrder(index, false);
    }

    void CreateNewOrder(int index, bool isCooldown)
    {
        orders[index] = generator.Generate(availableCollectibles, isCooldown);
        Save(index);
    }
}
