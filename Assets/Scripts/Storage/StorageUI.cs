using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnlimitedScrollUI;

//Component
public class StorageUI : SingletonMonoBehavior<StorageUI>
{
    [SerializeField] TextMeshProUGUI capacityText;
    [SerializeField] Button changeView;
    [SerializeField] TextMeshProUGUI changeViewLabel;
    [SerializeField] Slider capacitySlider;
    
    [SerializeField] GameObject storageView;
    [SerializeField] GameObject itemCell;
    [SerializeField] GridUnlimitedScroller scroller;
    
    [SerializeField] GameObject upgradeView;
    [SerializeField] Image[] upgradeToolIcons = new Image[3];
    [SerializeField] TextMeshProUGUI[] upgradeToolTexts = new TextMeshProUGUI[3];
    [SerializeField] Button upgradeButton;
    [SerializeField] GameObject buyUpgrade;
    [SerializeField] TextMeshProUGUI lackText;
    
    protected override void Awake()
    {
        base.Awake();
        changeView.onClick.AddListener(() => SetStorageViewState(!storageView.gameObject.activeSelf));
        upgradeButton.onClick.AddListener(StorageSystem.Instance.OnClickUpgrade);
        SetStorageViewState(false);
    }

    void Start()
    {
        gameObject.SetActive(false);
        transform.localScale = new Vector3(0, 0, 0);

        //upgradeButton.onClick.AddListener(StorageSystem.Instance.IncreaseCapacity);
    }

    public void OpenStorageUI()
    {
        UICurtain.Instance.AddListener(CloseStorageUI, false);
        gameObject.SetActive(true);
        gameObject.transform.DOScale(1, 0.2f)
            .SetEase(Ease.OutBack);
    }
    
    void CloseStorageUI()
    {
        UICurtain.Instance.RemoveListener(CloseStorageUI);
        gameObject.transform.DOScale(0, 0.2f)
            .OnComplete(() => gameObject.SetActive(false))
            .SetEase(Ease.InBack);
    }

    public void LoadStoringData(int currentCap, int maxCap, Dictionary<Collectible, int> items)
    {
        capacityText.text = currentCap + "/" + maxCap;
        capacitySlider.maxValue = maxCap;
        capacitySlider.value = currentCap;
        
        if (items.Count > 0) Generate(items);
    }

    public void LoadUpgradeData(Item[] tools)
    {
        int lackAmount = 0;
        for(int i = 0; i < 3; i++)
        {
            int storageToolAmount = StorageSystem.Instance.GetStoreAmount(tools[i].collectible);
            lackAmount += tools[i].amount - storageToolAmount;
            upgradeToolIcons[i].sprite = tools[i].collectible.icon;
            upgradeToolTexts[i].text = storageToolAmount +
                                       "/" +
                                       tools[i].amount;
        }

        lackText.text = (lackAmount * 10).ToString();
    }

    void Generate(Dictionary<Collectible, int> itemList)
    {
        scroller.ClearALlCells();
        scroller.Generate(itemCell, itemList.Count, (index, iCell) =>
        {
            StorageCell storageCell = iCell as StorageCell;
            if(storageCell != null) storageCell.AssignData
                (itemList.ElementAt(index).Key, itemList.ElementAt(index).Value);
        });
    }

    void SetStorageViewState(bool state)
    {
        upgradeView.gameObject.SetActive(!state);
        buyUpgrade.SetActive(state);
        storageView.gameObject.SetActive(state);

        changeViewLabel.text = state ? "Kho" : "Nâng cấp";
    }
}
