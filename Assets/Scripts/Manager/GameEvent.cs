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

public class FactoryDataEvent : GameEvent
{
    public readonly string factoryID;
    readonly string factoryDataID;
    readonly BoundsInt area;
    readonly int queueCapacity;
    readonly Queue<SavedProcessingData> processing;
    readonly Queue<string> completed; //productDataID of completed item

    public FactoryDataEvent(string factoryID, string factoryDataID, BoundsInt area, 
        Queue<ProductData> processing, Queue<ProductData> completed, int queueCapacity = 3)
    {
        this.factoryID = factoryID;
        this.factoryDataID = factoryDataID;
        this.area = area;
        this.queueCapacity = queueCapacity;
        this.processing = ConvertProcessing(processing);
        this.completed = ConvertCompleted(completed);
    }

    static Queue<SavedProcessingData> ConvertProcessing(Queue<ProductData> dataQueue)
    {
        Queue<SavedProcessingData> toReturn = new Queue<SavedProcessingData>();
        
        foreach(ProductData productData in dataQueue)
        {
            toReturn.Enqueue(productData.ConvertToSavedData());
        }
        
        return toReturn;
    }

    static Queue<string> ConvertCompleted(Queue<ProductData> dataQueue)
    {
        Queue<string> toReturn = new Queue<string>();
        
        foreach(ProductData productData in dataQueue)
        {
            toReturn.Enqueue(productData.id);
        }
        
        return toReturn;
    }

    public SavedFactoryData CreateSavedFactoryData()
    {
        return new SavedFactoryData(factoryID, factoryDataID, 
            area, queueCapacity, processing, completed);
    }
}