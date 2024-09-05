using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] bool isGem;
    [SerializeField] TextMeshProUGUI price;
    ShopItemData itemData;

    void Awake()
    {
        thisButton.onClick.AddListener(BuyItem);
    }

    public void AssignData(ShopItemData shopItem)
    {
        itemName.text = shopItem.itemName;
        icon.sprite = shopItem.icon;
        isGem = shopItem.currencyType == CurrencyType.Gold;
        price.text = shopItem.price.ToString();
        itemData = shopItem;

        if (!isGem) unit.sprite = ShopSystem.Instance.coin;
    }

    void BuyItem()
    {
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientCurrency);
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-itemData.price, itemData.currencyType));
    }

    void AllowBuy()
    {
        BuildingSystem.Instance.InstantiateConstruction(itemData.building);
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
