using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class ShopManager : SingletonMonoBehavior<ShopManager>, IGameSystem
{
    [SerializeField] RectTransform shop;
    [SerializeField] RectTransform shopButtonRect;
    [SerializeField] Button shopButton;

    [SerializedDictionary("Type", "Button")] 
    public SerializedDictionary<ItemType, TabButton> typeButtons;
    

    public Sprite coin;

    bool isOpen = false;
    bool isDragging;

    protected override void Awake()
    {
        base.Awake();
        
        shopButton.onClick.AddListener(OnClickShopButton);
    }
    
    public void StartingSystem()
    {
        EventManager.Instance.AddListener<LevelUpEvent>(OnLevelChanged);
        
        InitShopItem();
        gameObject.SetActive(false);
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

    void OpenCurtain()
    {
        
        UICurtain.Instance.Transparent();
        UICurtain.Instance.AssignOnClickOnce(() =>
        {
            CloseShop();
            UICurtain.Instance.TurnOff();
        });
    }

    void OpenShop()
    {
        if (isOpen) return;
        gameObject.SetActive(true);
        typeButtons[ItemType.AnimalHouses].thisButton.onClick.Invoke();
        OpenCurtain();
        shop.DOAnchorPosX(shop.anchoredPosition.x + shop.sizeDelta.x, 0.2f);
        shopButtonRect.DOAnchorPosX(shopButtonRect.anchoredPosition.x + shop.sizeDelta.x, 0.2f);
        isOpen = true;
    }

    public void CloseShop()
    {
        if (!isOpen) return;
        shop.DOAnchorPosX(shop.anchoredPosition.x - shop.sizeDelta.x, 0.2f)
            .OnComplete(() => gameObject.SetActive(false));
        shopButtonRect.DOAnchorPosX(shopButtonRect.anchoredPosition.x - shop.sizeDelta.x, 0.2f);
        isOpen = false;
    }

    void InitShopItem()
    {
        foreach(ItemType type in ResourceManager.Instance.shopItems.Keys)
        {
            typeButtons[type].SetUp(ResourceManager.Instance.shopItems[type]);
        }
    }

    //Dirty Code
    void OnLevelChanged(LevelUpEvent info)
    {
        for (int i = 0; i < ResourceManager.Instance.shopItems.Keys.Count; i++)
        {
            ItemType key = ResourceManager.Instance.shopItems.Keys.ToArray()[i];
            for (int j = 0; j < ResourceManager.Instance.shopItems[key].Count; j++)
            {
                ShopItem item = ResourceManager.Instance.shopItems[key][j];

                if (item.level == info.nextLv)
                {
                    //unlock
                }
            }
        }
    }
}