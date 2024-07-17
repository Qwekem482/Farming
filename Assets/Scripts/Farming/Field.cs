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
    
    CropData data;

    void Awake()
    {
        type = FactoryType.Field;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        switch (state)
        {
            case FactoryState.Idle:
                if (!ProductScroller.Instance.isOpen) ProductScroller.Instance.OpenScroller();
                break;
            case FactoryState.Processing:
                TimerSystem.Instance.ShowTimer(gameObject);
                break;
            case FactoryState.Complete:
                Sickle.Instance.ShowSickle();
                break;
            default:
                Debug.Log("OutOfRange");
                break;
        }
    }

    public void Plant(CropData cropData)
    {
        data = cropData;
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
        WaitForSeconds processingTime = new WaitForSeconds(data.processingTime.ToSecond());
        Timer.CreateTimer(gameObject, data.product.itemName, data.processingTime, OnSkipProcessingProduct);
        state = FactoryState.Processing;
        spriteRenderer.sprite = data.processingSprite;
        
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
        spriteRenderer.sprite = data.completeSprite;
    }

    public void HarvestProduct()
    {
        if (data == null) return;
        EventManager.Instance.QueueEvent(new StorageItemChangeEvent(new Item(data.product, 2)));
        EventManager.Instance.AddListenerOnce<SufficientCapacityEvent>(OnSufficient);
        EventManager.Instance.AddListenerOnce<InsufficientCapacityEvent>(OnInsufficient);
    }
    
    void OnSufficient(SufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<InsufficientCapacityEvent>(OnInsufficient);
        
        state = FactoryState.Idle;
        spriteRenderer.sprite = freeSprite;
        data = null;

        //Effect here
    }

    void OnInsufficient(InsufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientCapacityEvent>(OnSufficient);
        //Cancel collecting here
    }
}

