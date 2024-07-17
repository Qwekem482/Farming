using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectible", menuName = "CustomObject/Collectibles/Collectible")]
public class Collectible : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}

[Serializable]
public class Item
{
    public Collectible collectible;
    public int amount;

    public Item(Collectible collectible, int amount)
    {
        this.collectible = collectible;
        this.amount = amount;
    }

    public static Item[] CreateArrayItem(Collectible[] collectibles, int[] amount)
    {
        Item[] items = new Item[collectibles.Length];
        for (int i = 0; i < collectibles.Length; i++)
        {
            items[i] = new Item(collectibles[i], amount[i]);
        }
        return items;
    }
    
    public static Item[] CreateArrayItem(Collectible[] collectibles, int amount)
    {
        Item[] items = new Item[collectibles.Length];
        for (int i = 0; i < collectibles.Length; i++)
        {
            items[i] = new Item(collectibles[i], amount);
        }
        return items;
    }

    public static List<Item> ConvertToItem(Dictionary<Collectible, int> itemDict)
    {
        return itemDict.Select(item => new Item(item.Key, item.Value)).ToList();
    }

    public static Dictionary<Collectible, int> ConvertToDict(List<Item> itemList)
    {
        return itemList.ToDictionary(item => item.collectible, item => item.amount);
    }

    public static Item ConvertToNegativeAmount(Item item)
    {
        return new Item(item.collectible, -item.amount);
    }
}
