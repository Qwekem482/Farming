using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Order
{
    public readonly Item[] requestItems;
    public readonly int exp;
    public readonly int silver = 0;
    public readonly int gold = 0;
    public DateTime resetTime = default;

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
    
    public Order(Item[] requestItems, int exp, int currencyReward, bool isGoldenOrder, DateTime resetTime)
    {
        this.requestItems = requestItems;
        this.exp = exp;
        if (isGoldenOrder) gold = currencyReward;
        else silver = currencyReward;
    }
    
}
