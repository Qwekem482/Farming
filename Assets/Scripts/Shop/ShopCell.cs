using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;


public class ShopCell : RegularCell
{
    [SerializeField] Button thisButton;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Image icon;
    [SerializeField] Image unit;
    [SerializeField] TextMeshProUGUI price;
    
    bool isGem;
    bool isUnlocked;
    ShopBuildingData itemData;

    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }

        set
        {
            isUnlocked = value;
            thisButton.interactable = value;
            
            Color cellColor = value ? Color.white : Color.gray;
            itemName.color = cellColor;
            icon.color = cellColor;
            unit.color = cellColor;
            price.color = cellColor;
        }
    }

    void Awake()
    {
        thisButton.onClick.AddListener(BuyItem);
        icon.preserveAspect = true;
        IsUnlocked = false;
    }

    public void AssignData(ShopBuildingData shopItem)
    {
        itemName.text = shopItem.itemName;
        icon.sprite = shopItem.icon;
        isGem = shopItem.currencyType == CurrencyType.Gold;
        price.text = shopItem.price.ToString();
        itemData = shopItem;
        
        unit.sprite = !isGem ?
            ResourceManager.Instance.silverSprite :
            ResourceManager.Instance.goldSprite;
        
        IsUnlocked = LevelSystem.Instance.currentLevel >= shopItem.level;
    }

    void BuyItem()
    {
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientCurrency);
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-itemData.price, itemData.currencyType));
    }

    void AllowBuy()
    {
        BuildingSystem.Instance.InstantiateConstruction(itemData.building, itemData.price, itemData.currencyType);
        ShopSystem.Instance.CloseShop();
    }

    void OnSufficientCurrency(SufficientCurrencyEvent info)
    {
        AllowBuy();
        EventManager.Instance.RemoveListener<InsufficientCurrencyEvent>(OnInsufficientCurrency);
    }

    void OnInsufficientCurrency(InsufficientCurrencyEvent info)
    {
        Debug.Log("NotEnough");
        EventManager.Instance.RemoveListener<SufficientCurrencyEvent>(OnSufficientCurrency);
    }
}
