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
    ShopItem itemData;

    void Awake()
    {
        thisButton.onClick.AddListener(BuyItem);
    }

    public void AssignData(ShopItem shopItem)
    {
        itemName.text = shopItem.itemName;
        icon.sprite = shopItem.icon;
        isGem = shopItem.currencyType == CurrencyType.Gold;
        price.text = shopItem.price.ToString();
        itemData = shopItem;

        if (!isGem) unit.sprite = ShopManager.Instance.coin;
    }

    void BuyItem()
    {
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientCurrency);
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-itemData.price, itemData.currencyType));
    }

    void AllowBuy()
    {
        BuildingSystem.Instance.InstantiateConstruction(itemData);
        ShopManager.Instance.CloseShop();
    }

    void OnSufficientCurrency(SufficientCurrencyEvent info)
    {
        AllowBuy();
        EventManager.Instance.RemoveListener<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        UICurtain.Instance.TurnOff();
    }

    void OnInsufficientCurrency(InsufficientCurrencyEvent info)
    {
        Debug.Log("NotEnough");
        EventManager.Instance.RemoveListener<SufficientCurrencyEvent>(OnSufficientCurrency);
    }
}
