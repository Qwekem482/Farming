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

    List<ShopItemData> shopItems;
    public Button thisButton;

    void Awake()
    {
        thisButton.onClick.AddListener(OnClick);
    }

    public void SetUp(List<ShopItemData> items)
    {
        shopItems = items;
        totalCount = shopItems.Count;
        Debug.Log("shopItemCount: " + shopItems.Count + "|" + totalCount);
    }

    void OnClick()
    {
        scroller.Clear();
        Generate();
    }
    
    void Generate() 
    {
        scroller.Generate(cell, totalCount, (index, iCell) => {
            ShopCell shopCell = iCell as ShopCell;
            if (shopCell != null) shopCell.AssignData(shopItems[index]);
        });
    }
}
