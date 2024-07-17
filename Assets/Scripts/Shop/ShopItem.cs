using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShopItemData", menuName = "CustomObject/ShopItemData", order = 0)]
public class ShopItem : ScriptableObject
{
    //Showing Item
    public string itemName;
    public int level;
    public int price;
    public CurrencyType currencyType;
    public Sprite icon;
    
    //Init Item Data
    public BoundsInt area;
    public TimePeriod constructionTime;
    public Sprite buildingSprite;
    public FactoryType factoryType;
    
}

public enum ItemType
{
    AnimalHouses, //buildingShopItem
    Buildings, //buildingShopItem
    Trees,
    Decors,
}
