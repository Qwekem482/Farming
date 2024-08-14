using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Factory : ProductionBuilding
{
    Queue<ProductData> processingQueue;
    List<ProductData> completeQueue;
    
    public int queueCapacity = 3;

    Timer timer;
    void Awake()
    {
        processingQueue = new Queue<ProductData>();
        completeQueue = new List<ProductData>();
    }
    
    public override void Init(BuildingData data)
    {
        base.Init(data);
        uniqueID = SaveData.GenerateUniqueID();
        SaveState();
    }
    
    protected override void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!ProductScroller.Instance.isOpen) ProductScroller.Instance.OpenScroller(this, false);
        
        ReloadFactoryUIHolder(false);
        
        UnityAction onCompleteFocus = null;

        if (state == ProductionBuildingState.Processing)
        {
            onCompleteFocus = () =>
            {
                if (timer != null)
                    TimerUI.Instance.ShowTimer(gameObject);
            };
        }
        
        CameraSystem.Instance.Focus(
            transform.position, 
            onCompleteFocus);
    }
    
    protected override void SaveState()
    {
        DateTime productCompletedTime = DateTime.Now;
        Queue<SavedProcessingData> saveProcessing = new Queue<SavedProcessingData>();
        Queue<string> saveCompleted = new Queue<string>();
        
        if(processingQueue.Count > 0)
        {
            int index = 0;
            foreach(ProductData productData in processingQueue)
            {
                if (index == 0 && timer != null)
                {
                    productCompletedTime += TimeSpan.FromSeconds(timer.TimeLeft);
                } else
                {
                    productCompletedTime += productData.processingTime.ConvertToTimeSpan();
                }
                
                index++;
                saveProcessing.Enqueue(new SavedProcessingData(productData.id, productCompletedTime));
            }
        }
        
        if(completeQueue.Count > 0)
        {
            foreach(ProductData productData in completeQueue)
            {
                saveCompleted.Enqueue(productData.id);
            }
        }
        
        EventManager.Instance.QueueEvent(new SaveFactoryDataEvent
        (uniqueID, buildingData.id, transform.position, 
            buildingArea, saveProcessing, saveCompleted, queueCapacity));
    }
    
    public void LoadState(string factoryID, int queueCap, Queue<ProductData> savedProcessing, 
        IEnumerable<ProductData> savedCompleted, TimeSpan timeLeft = default)
    {
        uniqueID = factoryID;
        queueCapacity = queueCap;
        processingQueue = savedProcessing;
        completeQueue = savedCompleted.ToList();
        processingCoroutine = null;
        processingCoroutine = StartCoroutine(ProcessingProduct(timeLeft));
    }

    public override void AddProduct(ProductionOutputData inputData)
    {
        ProductData data = inputData as ProductData;
        
        if (data == null)
        {
            Debug.Log("Error data");
            return;
        }
        
        if (!StorageSystem.Instance.IsSufficient(data.materials))
        {
            Debug.Log("Insufficient Materials");
            return;
        }

        if (processingQueue.Count == queueCapacity)
        {
            Debug.Log("Insufficient Capacity");
            return;
        }
        
        foreach(Item item in data.materials)
        {
            EventManager.Instance.QueueEvent(new StorageItemChangeEvent(item.ConvertToNegativeAmount()));
            EventManager.Instance.AddListenerOnce<SufficientItemsEvent>(_ =>
            {
                ProductScroller.Instance.Clear();
                ProductScroller.Instance.Generate(this, false);
            });
        }
        
        processingQueue.Enqueue(data);
        processingCoroutine ??= StartCoroutine(ProcessingProduct());
        
        
        ReloadFactoryUIHolder();
        SaveState();
    }
    
    protected override IEnumerator ProcessingProduct(TimeSpan timeLeft = default)
    {
        state = ProductionBuildingState.Processing;
        while (processingQueue.Count > 0)
        {
            ProductData productData = processingQueue.Peek();

            WaitForSecondsRealtime processingTime = timeLeft == default ? 
                new WaitForSecondsRealtime(productData.processingTime.ToSecond()) :
                new WaitForSecondsRealtime((float) timeLeft.TotalSeconds);
            
            timer = Timer.CreateTimer(gameObject, productData.product.itemName, 
                productData.processingTime, null, OnSkipProcessingProduct, timeLeft);
            timeLeft = default;
            
            yield return new WaitForFixedUpdate();
            ReloadFactoryUIHolder();
            SaveState();
            
            yield return processingTime;
            OnCompleteProcessingProduct();
        }
        state = ProductionBuildingState.Idle;
        processingCoroutine = null;
    }

    protected override void OnSkipProcessingProduct()
    {
        if (processingCoroutine != null) StopCoroutine(processingCoroutine);
        OnCompleteProcessingProduct();
        
        /*state = ProductionBuildingState.Idle;
        processingCoroutine = null;
        processingCoroutine ??= StartCoroutine(ProcessingProduct());*/
    }

    protected override void OnCompleteProcessingProduct()
    {
        completeQueue.Add(processingQueue.Dequeue());
        SaveState();
        if (ReferenceEquals(FactoryUIHolder.Instance.currentFactory, this)) ReloadFactoryUIHolder();
    }

    public void ReloadFactoryUIHolder(bool showTimer = true)
    {
        FactoryUIHolder.Instance.Init(this, processingQueue, completeQueue);
        if (timer != null && showTimer)
            TimerUI.Instance.ShowTimer(gameObject);
    }

    public void RemoveCompletedData(ProductData productData)
    {
        completeQueue.Remove(productData);
        SaveState();
    }

    public override void Place()
    {
        base.Place();
        SaveState();
    }
}

