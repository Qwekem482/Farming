using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public readonly int amount;
    public readonly CurrencyType currencyType;

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

#region SaveEvent

#region SaveBuildingEvent

public class SaveBuildingDataEvent : GameEvent
{
    public string buildingID;
    protected string buildingDataID;
    protected Vector3 position;
    public BoundsInt area;

    public SaveBuildingDataEvent(string buildingID, string buildingDataID, Vector3 position, BoundsInt area)
    {
        this.buildingID = buildingID;
        this.buildingDataID = buildingDataID;
        this.position = position;
        this.area = area;
    }

    public SaveBuildingDataEvent()
    {
    }
    
    public SavedBuildingData CreateSavedBuildingData()
    {
        return new SavedBuildingData(buildingID, buildingDataID, position, 
            area);
    }
}

public class SaveProcessBuildingEvent : SaveBuildingDataEvent
{
    readonly DateTime completeTime;

    public SaveProcessBuildingEvent(string buildingID, string buildingDataID, 
        Vector3 position, BoundsInt area, DateTime completeTime)
    {
        this.buildingID = buildingID;
        this.buildingDataID = buildingDataID;
        this.position = position;
        this.area = area;
        this.completeTime = completeTime;
    }
    
    public SavedProcessBuildingData CreateSavedProcessBuildingData()
    {
        return new SavedProcessBuildingData(buildingID, buildingDataID, position, 
            area, completeTime);
    }
}

public class RemoveSaveProcessBuildingEvent : GameEvent
{
    public readonly string buildingID;
    public RemoveSaveProcessBuildingEvent(string buildingID)
    {
        this.buildingID = buildingID;
    }
}

public class SaveFactoryDataEvent : SaveBuildingDataEvent
{
    readonly int queueCapacity;
    readonly Queue<SavedProcessingData> processing;
    readonly Queue<string> completed; //productDataID of completed item

    public SaveFactoryDataEvent(string buildingID, string buildingDataID, Vector3 position, BoundsInt area, 
        Queue<SavedProcessingData> processing, Queue<string> completed, int queueCapacity = 3)
    {
        this.buildingID = buildingID;
        this.buildingDataID = buildingDataID;
        this.position = position;
        this.area = area;
        this.queueCapacity = queueCapacity;
        this.processing = processing;
        this.completed = completed;
    }

    public SavedFactoryData CreateSavedFactoryData()
    {
        return new SavedFactoryData(buildingID, buildingDataID, position, 
            area, queueCapacity, processing, completed);
    }
}

public class SaveFieldDataEvent : SaveBuildingDataEvent
{
    readonly SavedProcessingData processing;
    
    public SaveFieldDataEvent(string buildingID, string buildingDataID,
        Vector3 position, BoundsInt area, SavedProcessingData processing)
    {
        this.buildingID = buildingID;
        this.buildingDataID = buildingDataID;
        this.position = position;
        this.area = area;
        this.processing = processing;
    }
    public SaveFieldDataEvent(string buildingID, string buildingDataID,
        Vector3 position, BoundsInt area)
    {
        this.buildingID = buildingID;
        this.buildingDataID = buildingDataID;
        this.position = position;
        this.area = area;
        processing = null;
    }
    
    public SavedFieldData CreateSavedFieldData()
    {
        return new SavedFieldData(buildingID, buildingDataID, position, 
            area, processing);
    }
}

#endregion

#region OtherSaveEvent

public class SaveStorageCapacityEvent : GameEvent
{
    public readonly int capacity;
    public readonly int level;
    
    public SaveStorageCapacityEvent(int capacity, int level)
    {
        this.capacity = capacity;
        this.level = level;
    }
}

public class SaveCurrencyEvent : GameEvent
{
    public readonly int silver;
    public readonly int gold;

    public SaveCurrencyEvent(int silver, int gold)
    {
        this.silver = silver;
        this.gold = gold;
    }
}

public class SaveExpEvent : GameEvent
{
    public readonly int level;
    public readonly int currentExp;

    public SaveExpEvent(int level, int currentExp)
    {
        this.level = level;
        this.currentExp = currentExp;
    }
}

public class SaveOrderEvent : GameEvent
{
    public readonly int index;
    public readonly Order order;

    public SaveOrderEvent(int index, Order order)
    {
        this.index = index;
        this.order = order;
    }
}



#endregion

#endregion

