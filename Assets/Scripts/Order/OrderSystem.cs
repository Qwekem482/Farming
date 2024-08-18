using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : SingletonMonoBehavior<OrderSystem>, IGameSystem
{
    public List<Order> orderList;
    public int orderQuantity;

    OrderGenerator generator = new OrderGenerator();
    

    public void StartingSystem()
    {
        orderQuantity = Mathf.Clamp(LevelSystem.Instance.currentLevel, 1, 9);
    }

    public void SaveState()
    {
        
    }

    public void LoadState()
    {
        
    }

    public void CancelOrder(Order order)
    {
        ClearOrder(order);
    }

    public Order DeliveryOrder(Order order)
    {
        order.DeliveryOrder();
        ClearOrder(order);
        Order newOrder = generator.Generate();
        orderList.Add(newOrder);
        return newOrder;
    }

    #region PrivateMethod

    void GenerateOrder()
    {
        orderList.Add(generator.Generate());
    }

    void ClearOrder(Order order)
    {
        if (orderList.Contains(order)) orderList.Remove(order);
    }

    #endregion
}
