using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Field : Factory
{
    public Sprite freeSprite;
    SpriteRenderer spriteRenderer;
    
    CropData cropData;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        switch (state)
        {
            case FactoryState.Idle:
                if (!ProductScroller.Instance.isOpen) ProductScroller.Instance.OpenScroller(this, true);
                break;
            case FactoryState.Processing:
                TimerUI.Instance.ShowTimer(gameObject);
                break;
            case FactoryState.Complete:
                HorizontalUIHolder.Instance.OpenUI(true);
                break;
            default:
                Debug.Log("OutOfRange");
                break;
        }
    }

    public void Plant(CropData cropData)
    {
        this.cropData = cropData;
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientCurrency);
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-cropData.price, CurrencyType.Silver));
    }

    void OnSufficientCurrency(SufficientCurrencyEvent info)
    {
        EventManager.Instance.RemoveListener<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        processingCoroutine ??= StartCoroutine(ProcessingProduct());
    }
    
    void OnInsufficientCurrency(InsufficientCurrencyEvent info)
    {
        Debug.Log("NotEnough");
        EventManager.Instance.RemoveListener<SufficientCurrencyEvent>(OnSufficientCurrency);
    }

    IEnumerator ProcessingProduct()
    {
        WaitForSeconds processingTime = new WaitForSeconds(cropData.processingTime.ToSecond());
        Timer.CreateTimer(gameObject, cropData.product.itemName, cropData.processingTime, OnSkipProcessingProduct);
        state = FactoryState.Processing;
        spriteRenderer.sprite = cropData.processingSprite;
        
        yield return processingTime;
        
        OnCompleteProcessingProduct();
    }

    void OnSkipProcessingProduct()
    {
        StopCoroutine(processingCoroutine);
        OnCompleteProcessingProduct();
    }

    void OnCompleteProcessingProduct()
    {
        state = FactoryState.Complete;
        processingCoroutine = null;
        spriteRenderer.sprite = cropData.completeSprite;
    }

    public void HarvestProduct()
    {
        if (cropData == null) return;
        EventManager.Instance.QueueEvent(new StorageItemChangeEvent(new Item(cropData.product, 2)));
        EventManager.Instance.AddListenerOnce<SufficientCapacityEvent>(OnSufficient);
        EventManager.Instance.AddListenerOnce<InsufficientCapacityEvent>(OnInsufficient);
    }
    
    void OnSufficient(SufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<InsufficientCapacityEvent>(OnInsufficient);
        
        state = FactoryState.Idle;
        spriteRenderer.sprite = freeSprite;
        cropData = null;

        //Effect here
    }

    void OnInsufficient(InsufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientCapacityEvent>(OnSufficient);
        //Cancel collecting here
    }
}

