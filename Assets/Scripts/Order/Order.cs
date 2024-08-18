using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public Item[] requestItems;
    public int exp;
    public int silver = 0;
    public int gold = 0;
    public DateTime endTime;

    public bool CanBeDelivery()
    {
        return StorageSystem.Instance.IsSufficient(requestItems);
    }

    public void DeliveryOrder()
    {
        if (!CanBeDelivery()) return;
        
        foreach(Item item in requestItems)
        {
            EventManager.Instance.QueueEvent(new StorageItemChangeEvent(item.ConvertToNegativeAmount()));
        }
        
        OnDelivery();
    }
    
    void OnDelivery()
    {
        EventManager.Instance.QueueEvent(new ExpAddedEvent(exp));
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(silver, CurrencyType.Silver));
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(gold, CurrencyType.Gold));
    }

    public Order(Item[] requestItems, int exp, int currencyReward, bool isGoldenOrder)
    {
        this.requestItems = requestItems;
        this.exp = exp;
        if (isGoldenOrder) gold = currencyReward;
        else silver = currencyReward;
    }
    
}
