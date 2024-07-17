using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ProductUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected RectTransform rectTrans;
    protected Image icon;
    ProductData data;

    protected Vector2 originalPos;
    protected Canvas canvas;
    protected RaycastHit2D hit;
    protected ProductInfoCell cellParent;
    Factory factory;

    protected void Awake()
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        icon = gameObject.GetComponent<Image>();
    }
    
    public void Init(Canvas gameCanvas, ProductData productData, Factory currentFactory, ProductInfoCell parent)
    {
        rectTrans = gameObject.GetComponent<RectTransform>();
        icon = gameObject.GetComponent<Image>();
        canvas = gameCanvas;
        data = productData;
        icon.sprite = productData.product.icon;
        factory = currentFactory;
        cellParent = parent;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        PanZoom.Instance.enabled = false;
        originalPos = rectTrans.anchoredPosition;
        transform.SetParent(canvas.gameObject.transform);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
        //Poor logic
        if(ProductScroller.Instance.isOpen && 
           rectTrans.anchoredPosition.x > - canvas.gameObject.GetComponent<RectTransform>().sizeDelta.x * 2 / 3)
        {
            ProductScroller.Instance.CloseScroller();
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Detect()) factory.AddProcessingProduct(data);
        
        //rectTrans.anchoredPosition = originalPos;
        Destroy(gameObject);
        PanZoom.Instance.enabled = true;
    }
    
    bool Detect()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(touchPos, Vector2.zero);

        return hit.collider != null && ReferenceEquals(hit.collider.gameObject, factory.gameObject);
    }
}
