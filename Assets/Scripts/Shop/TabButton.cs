using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;


[RequireComponent(typeof(Button))]
public class TabButton : MonoBehaviour
{
    [SerializeField] VerticalUnlimitedScroller scroller;
    [SerializeField] GameObject cell;
    int totalCount = 0;

    List<ShopBuildingData> shopItems;
    public Button button;
    public Image icon;

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetUp(List<ShopBuildingData> items)
    {
        shopItems = items;
        totalCount = shopItems.Count;
    }

    void OnClick()
    {
        scroller.Clear();
        Generate();

        foreach(TabButton tabButton in ShopSystem.Instance.tabButtons.Values)
        {
            tabButton.icon.color = new Color((float)110/255, (float)85/255, (float)74/255);
        }
        
        icon.color = new Color((float)195/255, (float)157/255, (float)121/255);
    }
    
    void Generate() 
    {
        scroller.Generate(cell, totalCount, (index, iCell) => {
            ShopCell shopCell = iCell as ShopCell;
            if (shopCell != null) shopCell.AssignData(shopItems[index]);
        });
    }
}
