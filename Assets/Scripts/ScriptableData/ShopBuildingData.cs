using UnityEngine;


[CreateAssetMenu(fileName = "ShopItemData", menuName = "CustomObject/ShopItemData", order = 0)]
public class ShopBuildingData : ScriptableObject
{
    //Showing Item
    public string itemName;
    public int price;
    public int level;
    public CurrencyType currencyType;
    public Sprite icon;
    public BuildingData building;

    void OnValidate()
    {
        if (building != null)
        {
            itemName = building.buildingName;
            icon = building.sprite;
        }
    }
}

public enum BuildingType
{
    AnimalHouses,
    Factory,
    Trees,
    Decors,
}



