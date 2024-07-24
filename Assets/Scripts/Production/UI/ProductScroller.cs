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
    [SerializeField] SerializedDictionary<BuildingType, ProductData[]> products;
    [SerializeField] VerticalUnlimitedScroller scroller;
    [SerializeField] GameObject cell;
    [SerializeField] RectTransform rectTrans;
    [SerializeField] Canvas canvas;
    [SerializeField] Sprite coin;

    Factory factory;
    public bool isOpen;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void OpenCurtain()
    {
        UICurtain.Instance.Transparent();
        UICurtain.Instance.AssignOnClickOnce(() =>
        {
            CloseScroller();
            UICurtain.Instance.TurnOff();
        });
    }

    public void OpenScroller(Factory currentFactory)
    {
        isOpen = true;
        factory = currentFactory;
        gameObject.SetActive(true);
        Generate(currentFactory);
        OpenCurtain();
        rectTrans.DOAnchorPosX(rectTrans.anchoredPosition.x + rectTrans.sizeDelta.x, 0.2f);
    }

    public void OpenScroller()
    {
        isOpen = true;
        Generate();
        gameObject.SetActive(true);
        OpenCurtain();
        rectTrans.DOAnchorPosX(rectTrans.anchoredPosition.x + rectTrans.sizeDelta.x, 0.2f);
    }

    public void CloseScroller()
    {
        isOpen = false;
        factory = null;
        rectTrans.DOAnchorPosX(rectTrans.anchoredPosition.x - rectTrans.sizeDelta.x, 0.2f)
            .OnComplete(() =>
            {
                Clear();
                gameObject.SetActive(false);
            });
    }

    void Generate(Factory currentFactory)
    {
        scroller.Generate(cell, products[currentFactory.type].Length, (index, iCell) =>
        {
            ProductInfoCell infoCell = iCell as ProductInfoCell;
            if (infoCell != null) infoCell.AssignData(products[currentFactory.type][index], canvas, factory);
        });
    }

    void Generate()
    {
        scroller.Generate(cell, products[BuildingType.Field].Length, (index, iCell) =>
        {
            ProductInfoCell infoCell = iCell as ProductInfoCell;
            if (infoCell != null) infoCell.AssignData(products[BuildingType.Field][index] as CropData, canvas, coin);
        });
    }

    void Clear()
    {
        scroller.Clear();
    }
}
