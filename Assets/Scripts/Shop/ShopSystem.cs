using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class ShopSystem : SingletonMonoBehavior<ShopSystem>, IGameSystem
{
    [SerializeField] RectTransform shop;
    [SerializeField] RectTransform shopButtonRect;
    [SerializeField] Button shopButton;

    [SerializedDictionary("Type", "Button")] 
    public SerializedDictionary<BuildingType, TabButton> typeButtons;

    bool isOpen = false;
    bool isDragging;

    protected override void Awake()
    {
        base.Awake();
        shopButton.onClick.AddListener(OnClickShopButton);
        //EventManager.Instance.AddListener<LevelUpEvent>(OnLevelChanged);
    }
    
    public void StartingSystem()
    {
        InitShopItem();
        shop.gameObject.SetActive(false);
    }

    void OnClickShopButton()
    {
        if (!isOpen)
        {
            OpenShop();
        } else
        {
            CloseShop();
        }
    }
    

    void OpenShop()
    {
        if (isOpen) return;
        shop.gameObject.SetActive(true);
        typeButtons[BuildingType.Factory].thisButton.onClick.Invoke();
        UICurtain.Instance.AddListener(CloseShop);
        shop.DOAnchorPosX(shop.anchoredPosition.x + shop.sizeDelta.x, 0.2f);
        shopButtonRect.DOAnchorPosX(shopButtonRect.anchoredPosition.x + shop.sizeDelta.x, 0.2f);
        isOpen = true;
    }

    public void CloseShop()
    {
        if (!isOpen) return;
        UICurtain.Instance.RemoveListener(CloseShop);
        shop.DOAnchorPosX(shop.anchoredPosition.x - shop.sizeDelta.x, 0.2f)
            .OnComplete(() => shop.gameObject.SetActive(false));
        shopButtonRect.DOAnchorPosX(shopButtonRect.anchoredPosition.x - shop.sizeDelta.x, 0.2f);
        isOpen = false;
    }

    void InitShopItem()
    {
        foreach(BuildingType type in ResourceManager.Instance.shopItems.Keys)
        {
            typeButtons[type].SetUp(ResourceManager.Instance.shopItems[type].ToList());
        }
    }
}