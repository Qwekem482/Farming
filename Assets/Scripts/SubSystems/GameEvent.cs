using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent { }

public class CurrencyChangeEvent : GameEvent
{
    public int amount;
    public CurrencyType currencyType;

    public CurrencyChangeEvent(int amount, CurrencyType currencyType)
    {
        this.amount = amount;
        this.currencyType = currencyType;
    }
}

public class InsufficientCurrencyEvent : GameEvent
{
    public int amount;
    public CurrencyType currencyType;

    public InsufficientCurrencyEvent(int amount, CurrencyType currencyType)
    {
        this.amount = amount;
        this.currencyType = currencyType;
    }
}

public class SufficientCurrencyEvent : GameEvent
{
    public int amount;
    public CurrencyType currencyType;

    public SufficientCurrencyEvent(int amount, CurrencyType currencyType)
    {
        this.amount = amount;
        this.currencyType = currencyType;
    }
}

public class ExpAddedEvent : GameEvent
{
    public int amount;

    public ExpAddedEvent(int amount)
    {
        this.amount = amount;
    }
}

public class LevelUpEvent : GameEvent
{
    public int nextLv;

    public LevelUpEvent(int nextLv)
    {
        this.nextLv = nextLv;
    }
}

public class StorageItemChangeEvent : GameEvent
{
    public Item item;

    public StorageItemChangeEvent(Item item)
    {
        this.item = item;
    }
}

public class InsufficientItemsEvent : GameEvent
{
    public Item item;
    
    public InsufficientItemsEvent(Item item)
    {
        this.item = item;
    }
}

public class SufficientItemsEvent : GameEvent
{
    public Item item;
    
    public SufficientItemsEvent(Item item)
    {
        this.item = item;
    }
}

public class InsufficientCapacityEvent : GameEvent
{
    public Item item;
    
    public InsufficientCapacityEvent(Item item)
    {
        this.item = item;
    }
}

public class SufficientCapacityEvent : GameEvent
{
    public Item item;
    
    public SufficientCapacityEvent(Item item)
    {
        this.item = item;
    }
}




