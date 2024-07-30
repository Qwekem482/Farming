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

    public void OpenScroller(Factory currentFactory, bool isField)
    {
        isOpen = true;
        factory = currentFactory;
        gameObject.SetActive(true);
        Generate(currentFactory, isField);
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

    void Generate(Factory currentFactory, bool isField)
    {
        
        scroller.Generate(cell, currentFactory.factoryData.productData.Count, (index, iCell) =>
        {
            ProductInfoCell infoCell = iCell as ProductInfoCell;
            if (infoCell == null) return;
            if (isField)
                infoCell.AssignData(currentFactory.factoryData.productData[index] as CropData, canvas, coin);
            else infoCell.AssignData(currentFactory.factoryData.productData[index], canvas, factory);
        });
    }

    void Clear()
    {
        scroller.Clear();
    }
}
