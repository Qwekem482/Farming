using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : SingletonMonoBehavior<OrderSystem>, IGameSystem
{
    [NonSerialized]public Order[] orders = new Order[9];
    [NonSerialized]public int orderQuantity = 9;

    [NonSerialized]readonly OrderGenerator generator = new OrderGenerator();
    

    public void StartingSystem()
    {
        
    }
    
    void Save(int index)
    {
        EventManager.Instance.QueueEvent(new SaveOrderEvent(index, orders[index]));
    }
    
    public void Load(Order[] orderArray)
    {
        orders = orderArray;
    }

    public void CancelOrder(int index)
    {
        CreateNewOrder(index);
    }

    public void DeliveryOrder(int index)
    {
        if (!orders[index].CanBeDelivery()) return;
        CreateNewOrder(index);
    }

    void CreateNewOrder(int index)
    {
        orders[index] = generator.Generate(true);
        Save(index);
    }
}
