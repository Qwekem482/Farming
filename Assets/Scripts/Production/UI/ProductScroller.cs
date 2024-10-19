using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnlimitedScrollUI;

public class ProductScroller : SingletonMonoBehavior<ProductScroller>
{
    [SerializeField] VerticalUnlimitedScroller scroller;
    [SerializeField] GameObject factoryCell;
    [SerializeField] GameObject fieldCell;
    [SerializeField] RectTransform rectTrans;
    [SerializeField] Canvas canvas;

    ProductionBuilding factory;
    public bool isOpen;

    bool buildingIsField;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenScroller(ProductionBuilding currentFactory, bool isField)
    {
        if (isOpen) return;
        isOpen = true;
        buildingIsField = isField;
        factory = currentFactory;
        gameObject.SetActive(true);
        Generate(currentFactory, isField);
        UICurtain.Instance.AddListener(CloseScroller);
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
        UICurtain.Instance.RemoveListener(CloseScroller);
    }

    public void Generate(ProductionBuilding currentFactory, bool isField)
    {
        ProductionBuildingData data;
        
        try
        {
            data = (ProductionBuildingData) currentFactory.buildingData;
        }
        catch (Exception e)
        {
            Debug.LogError("Cannot cast to FactoryData" + "\n" + e.Message + "\n" + e.StackTrace);
            throw;
        }

        if (isField)
        {
            scroller.Generate(fieldCell, data.productData.Count, (index, iCell) =>
            {
                FieldProductInfoCell infoCell = iCell as FieldProductInfoCell;
                if (infoCell == null) return; 
                infoCell.AssignData(data.productData[index] as CropData, canvas);
            });
        } else
        {
            scroller.Generate(factoryCell, data.productData.Count, (index, iCell) =>
            {
                FactoryProductInfoCell infoCell = iCell as FactoryProductInfoCell;
                if (infoCell == null) return;
                infoCell.AssignData(data.productData[index] as ProductData, canvas, factory as Factory);
            });
        }
    }

    public void Clear()
    {
        scroller.Clear();
    }
}
