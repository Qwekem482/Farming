using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class ShopManager : SingletonMonoBehavior<ShopManager>
{
    [SerializeField] RectTransform shop;
    [SerializeField] RectTransform shopButtonRect;
    [SerializeField] Button shopButton;
    
    [SerializedDictionary("Type", "List")] 
    public SerializedDictionary<ItemType, List<ShopItem>> shopItems;

    [SerializedDictionary("Type", "Button")] 
    public SerializedDictionary<ItemType, TabButton> typeButtons;
    
    [SerializeField] UICurtain curtain;

    public Sprite coin;

    bool isOpen = false;
    bool isDragging;

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.AddListener<LevelUpEvent>(OnLevelChanged);
        shopButton.onClick.AddListener(OnClickShopButton);
    }
    
    void Start()
    {
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

    void OnEnable()
    {
        curtain.Transparent();
        curtain.AssignOnClickOnce(() =>
        {
            CloseShop();
            curtain.TurnOff();
        });
    }

    void OpenShop()
    {
        gameObject.SetActive(true);
        typeButtons[ItemType.AnimalHouses].thisButton.onClick.Invoke();
        shop.DOAnchorPosX(shop.anchoredPosition.x + shop.sizeDelta.x, 0.2f);
        shopButtonRect.DOAnchorPosX(shopButtonRect.anchoredPosition.x + shop.sizeDelta.x, 0.2f);
        isOpen = true;
    }

    public void CloseShop()
    {
        shop.DOAnchorPosX(shop.anchoredPosition.x - shop.sizeDelta.x, 0.2f)
            .OnComplete(() => gameObject.SetActive(false));
        shopButtonRect.DOAnchorPosX(shopButtonRect.anchoredPosition.x - shop.sizeDelta.x, 0.2f);
        isOpen = false;
    }

    void InitShopItem()
    {
        foreach(ItemType type in shopItems.Keys)
        {
            typeButtons[type].SetUp(shopItems[type]);
        }
    }

    void OnLevelChanged(LevelUpEvent info)
    {
        for (int i = 0; i < shopItems.Keys.Count; i++)
        {
            ItemType key = shopItems.Keys.ToArray()[i];
            for (int j = 0; j < shopItems[key].Count; j++)
            {
                ShopItem item = shopItems[key][j];

                if (item.level == info.nextLv)
                {
                    //unlock
                }
            }
        }
    }
}