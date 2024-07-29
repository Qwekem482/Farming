using UnityEngine;


[CreateAssetMenu(fileName = "ShopItemData", menuName = "CustomObject/ShopItemData", order = 0)]
public class ShopItemData : ScriptableObject
{
    //Showing Item
    public string itemName;
    public int level;
    public int price;
    public CurrencyType currencyType;
    public ItemType itemType;
    public Sprite icon;
    public BuildingData building;

    void OnValidate()
    {
        itemName = building.buildingName;
    }
}

public enum ItemType
{
    AnimalHouses, //buildingShopItem
    Factory, //buildingShopItem
    Trees,
    Decors,
}
