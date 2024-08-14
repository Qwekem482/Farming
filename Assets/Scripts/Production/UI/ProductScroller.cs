using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnlimitedScrollUI;

public class ProductScroller : SingletonMonoBehavior<ProductScroller>
{
    [SerializeField] VerticalUnlimitedScroller scroller;
    [SerializeField] GameObject cell;
    [SerializeField] RectTransform rectTrans;
    [SerializeField] Canvas canvas;
    [SerializeField] Sprite coin;

    ProductionBuilding factory;
    public bool isOpen;

    bool buildingIsField;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OpenCurtain()
    {
        UICurtain.Instance.Transparent();
        UICurtain.Instance.AssignOnClickOnce(CloseScroller);
    }

    public void OpenScroller(ProductionBuilding currentFactory, bool isField)
    {
        if (isOpen) return;
        isOpen = true;
        buildingIsField = isField;
        factory = currentFactory;
        gameObject.SetActive(true);
        Generate(currentFactory, isField);
        OpenCurtain();
        rectTrans.DOAnchorPosX(rectTrans.anchoredPosition.x + rectTrans.sizeDelta.x, 0.2f);
    }
    

    public void CloseScroller()
    {
        if (!isOpen) return;
        isOpen = false;
        factory = null;
        rectTrans.DOAnchorPosX(rectTrans.anchoredPosition.x - rectTrans.sizeDelta.x, 0.2f)
            .OnComplete(() =>
            {
                Clear();
                gameObject.SetActive(false);
            });
    }

    public void Generate(ProductionBuilding currentFactory, bool isField)
    {
        ProductionBuildingData factoryData;
        
        try
        {
            factoryData = (ProductionBuildingData) currentFactory.buildingData;
        }
        catch (Exception e)
        {
            Debug.LogError("Cannot cast to FactoryData" + "\n" + e.Message + "\n" + e.StackTrace);
            throw;
        }
            
        scroller.Generate(cell, factoryData.productData.Count, (index, iCell) =>
        {
            ProductInfoCell infoCell = iCell as ProductInfoCell;
            if (infoCell == null) return;
            if (isField)
                infoCell.AssignData(factoryData.productData[index] as CropData, canvas, coin);
            else infoCell.AssignData(factoryData.productData[index] as ProductData, canvas, factory as Factory);
        });
    }

    public void Clear()
    {
        scroller.Clear();
    }
}
