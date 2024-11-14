using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class ShopSystem : SingletonMonoBehavior<ShopSystem>, IGameSystem
{
    [SerializeField] RectTransform shop;
    [SerializeField] Button shopButton;

    [SerializedDictionary("Type", "Button")] 
    public SerializedDictionary<BuildingType, TabButton> tabButtons;

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
        InitShopItem();
        tabButtons[BuildingType.Factory].button.onClick.Invoke();
        UICurtain.Instance.AddListener(CloseShop);
        shop.DOAnchorPosX(shop.anchoredPosition.x + shop.sizeDelta.x, 0.2f);
        isOpen = true;
    }

    public void CloseShop()
    {
        if (!isOpen) return;
        UICurtain.Instance.RemoveListener(CloseShop);
        shop.DOAnchorPosX(shop.anchoredPosition.x - shop.sizeDelta.x, 0.2f)
            .OnComplete(() => shop.gameObject.SetActive(false));
        isOpen = false;
    }

    void InitShopItem()
    {
        foreach(BuildingType type in ResourceManager.Instance.shopItems.Keys)
        {
            tabButtons[type].SetUp(ResourceManager.Instance.shopItems[type].ToList());
        }
    }
}