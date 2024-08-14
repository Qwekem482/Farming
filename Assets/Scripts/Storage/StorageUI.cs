using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

//Component
public class StorageUI : SingletonMonoBehavior<StorageUI>
{
    [SerializeField] TextMeshProUGUI capacityText;
    [SerializeField] Button changeView;
    [SerializeField] Button closeButton;
    [SerializeField] Slider capacitySlider;
    
    [SerializeField] GameObject storageView;
    [SerializeField] GameObject itemCell;
    [SerializeField] GridUnlimitedScroller scroller;
    
    [SerializeField] GameObject upgradeView;
    [SerializeField] Image[] upgradeToolIcons = new Image[3];
    [SerializeField] TextMeshProUGUI[] upgradeToolTexts = new TextMeshProUGUI[3];
    [SerializeField] Button upgradeButton;
    [SerializeField] TextMeshProUGUI upgradeButtonText;
    [SerializeField] TextMeshProUGUI newCapacityText;
    
    protected override void Awake()
    {
        base.Awake();
        closeButton.onClick.AddListener(() => UICurtain.Instance.InvokeAndClose());
        changeView.onClick.AddListener(() => SetStorageViewState(!storageView.gameObject.activeSelf));
        upgradeButton.onClick.AddListener(StorageSystem.Instance.OnClickUpgrade);
        SetStorageViewState(false);
    }

    void Start()
    {
        gameObject.SetActive(false);
        //upgradeButton.onClick.AddListener(StorageSystem.Instance.IncreaseCapacity);
    }

    public void OpenStorageUI()
    {
        UICurtain.Instance.DarkFade();
        UICurtain.Instance.AssignOnClickOnce(CloseStorageUI);
        
        gameObject.SetActive(true);
    }
    
    void CloseStorageUI()
    {
        gameObject.SetActive(false);
    }

    public void LoadStoringData(int currentCap, int maxCap, Dictionary<Collectible, int> items)
    {
        capacityText.text = "Capacity: " + currentCap + "/" + maxCap;
        capacitySlider.maxValue = maxCap;
        capacitySlider.value = currentCap;
        
        if (items.Count > 0) Generate(items);
    }

    public void LoadUpgradeData(Item[] tools, int maxCapacity)
    {
        int lackAmount = 0;
        for(int i = 0; i < 3; i++)
        {
            int storageToolAmount = StorageSystem.Instance.GetCollectibleStoreAmount(tools[i].collectible);
            lackAmount += tools[i].amount - storageToolAmount;
            upgradeToolIcons[i].sprite = tools[i].collectible.icon;
            upgradeToolTexts[i].text = storageToolAmount +
                                       "/" +
                                       tools[i].amount;
        }

        upgradeButtonText.text = "Upgrade Now: " + lackAmount * 10;
        newCapacityText.text = "Increase Capacity To " + (maxCapacity + 50);
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
        storageView.gameObject.SetActive(state);
    }
}
