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
        cropData = null;
    }
    
    public override void Init(BuildingData data, BoundsInt area)
    {
        base.Init(data, area);
        IsPlaced = true;
        freeSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        uniqueID = SaveData.GenerateUniqueID();
        SaveState();
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!IsPlaced) return;

        switch (state)
        {
            case ProductionBuildingState.Idle:
                if (!ProductScroller.Instance.isOpen) 
                    ProductScroller.Instance.OpenScroller(this, true);
                break;
            case ProductionBuildingState.Processing:
                TimerUI.Instance.ShowTimer(gameObject);
                break;
            case ProductionBuildingState.Complete:
                HorizontalUIHolder.Instance.OpenUI(true);
                break;
            default:
                Debug.Log("OutOfRange");
                break;
        }
    }
    
    protected override void SaveState()
    {
        if (cropData == null)
        {
            EventManager.Instance.QueueEvent(new SaveFieldDataEvent(
                uniqueID, buildingData.id, transform.position,
                buildingArea));
        } else
        {
            Timer timer = gameObject.GetComponent<Timer>();
            DateTime completedDateTime;
            
            if(timer == null) completedDateTime = DateTime.Now;
            else completedDateTime = DateTime.Now + TimeSpan.FromSeconds(timer.TimeLeft);
            
            EventManager.Instance.QueueEvent(new SaveFieldDataEvent(
                uniqueID, buildingData.id, transform.position,
                buildingArea, new SavedProcessingData(cropData.productID, completedDateTime)));
        }
    }

    public void LoadState(string fieldID, CropData data, TimeSpan timeLeft = default)
    {
        uniqueID = fieldID;
        cropData = data;
        IsPlaced = true;

        if (cropData == null)
        {
            state = ProductionBuildingState.Idle;
            return;
        }
        

        if (timeLeft == default)
        {
            OnCompleteProcessingProduct();
            return;
        }

        processingCoroutine = StartCoroutine(ProcessingProduct(timeLeft));
    }

    protected override IEnumerator ProcessingProduct(TimeSpan timeLeft = default)
    {
        WaitForSecondsRealtime processingTime = new WaitForSecondsRealtime(cropData.processingTime.ToSecond());
        Timer.CreateTimer(gameObject, cropData.product.itemName,
            cropData.processingTime, null, OnSkipProcessingProduct, timeLeft);
        
        state = ProductionBuildingState.Processing;
        spriteRenderer.sprite = cropData.processingSprite;

        yield return processingTime;
        OnCompleteProcessingProduct();
    }

    protected override void OnSkipProcessingProduct()
    {
        if (processingCoroutine != null) StopCoroutine(processingCoroutine);
        OnCompleteProcessingProduct();
    }

    protected override void OnCompleteProcessingProduct()
    {
        state = ProductionBuildingState.Complete;
        processingCoroutine = null;
        spriteRenderer.sprite = cropData.completeSprite;
        SaveState();
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
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-cropData.inputPrice, CurrencyType.Silver));
    }

    public void HarvestProduct()
    {
        if (cropData == null) return;
        EventManager.Instance.QueueEvent(new StorageItemChangeEvent(new Item(cropData.product, 2)));
        EventManager.Instance.AddListenerOnce<SufficientCapacityEvent>(OnSufficientStorageCapacity);
        EventManager.Instance.AddListenerOnce<InsufficientCapacityEvent>(OnInsufficientStorageCapacity);
    }
    

    #region EventCommunication
    
    void OnSufficientCurrency(SufficientCurrencyEvent info)
    {
        EventManager.Instance.RemoveListener<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        processingCoroutine ??= StartCoroutine(ProcessingProduct());
        SaveState();
    }
    
    void OnInsufficientCurrency(InsufficientCurrencyEvent info)
    {
        Debug.Log("NotEnough");
        EventManager.Instance.RemoveListener<SufficientCurrencyEvent>(OnSufficientCurrency);
    }

    void OnSufficientStorageCapacity(SufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<InsufficientCapacityEvent>(OnInsufficientStorageCapacity);
        
        state = ProductionBuildingState.Idle;
        spriteRenderer.sprite = freeSprite;
        cropData = null;
        SaveState();

        //Effect here
    }

    void OnInsufficientStorageCapacity(InsufficientCapacityEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientCapacityEvent>(OnSufficientStorageCapacity);
        //Cancel collecting here
    }

    #endregion
}

