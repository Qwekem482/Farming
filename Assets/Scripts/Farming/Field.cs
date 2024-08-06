using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Field : ProductionBuilding
{
    public Sprite freeSprite;
    SpriteRenderer spriteRenderer;
    
    CropData cropData;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public override void Init(BuildingData data)
    {
        base.Init(data);
        freeSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        SaveState();
    }

    protected override void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        switch (state)
        {
            case FactoryState.Idle:
                if (!ProductScroller.Instance.isOpen) 
                    ProductScroller.Instance.OpenScroller(this, true);
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
    
    protected override void SaveState()
    {
        DateTime completedDateTime =
            DateTime.Now +
            TimeSpan.FromSeconds(gameObject.GetComponent<Timer>().TimeLeft);
        
        EventManager.Instance.QueueEvent(new SaveFieldDataEvent(
            uniqueID, buildingData.id, transform.position,
            buildingArea, new SavedProcessingData(cropData.id, completedDateTime)));
    }

    public void LoadState()
    {
        
    }

    protected override IEnumerator ProcessingProduct(TimeSpan timeLeft = default)
    {
        WaitForSeconds processingTime = new WaitForSeconds(cropData.processingTime.ToSecond());
        Timer.CreateTimer(gameObject, cropData.product.itemName,
            cropData.processingTime, OnSkipProcessingProduct, timeLeft);
        
        state = FactoryState.Processing;
        spriteRenderer.sprite = cropData.processingSprite;
        
        yield return processingTime;
        
        OnCompleteProcessingProduct();
    }

    protected override void OnSkipProcessingProduct()
    {
        StopCoroutine(processingCoroutine);
        OnCompleteProcessingProduct();
    }

    protected override void OnCompleteProcessingProduct()
    {
        state = FactoryState.Complete;
        processingCoroutine = null;
        spriteRenderer.sprite = cropData.completeSprite;
    }
    
    public override void AddProduct(ProductionOutputData inputData)
    {
        cropData = inputData as CropData;

        if (cropData == null)
        {
            Debug.Log("Error data");
            return;
        }
        
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientCurrency);
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-cropData.price, CurrencyType.Silver));
    }

    public void HarvestProduct()
    {
        if (cropData == null) return;
        EventManager.Instance.QueueEvent(new StorageItemChangeEvent(new Item(cropData.product, 2)));
        EventManager.Instance.AddListenerOnce<SufficientCapacityEvent>(OnSufficient);
        EventManager.Instance.AddListenerOnce<InsufficientCapacityEvent>(OnInsufficient);
    }
    

    #region EventCommunication
    
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

    #endregion
}

