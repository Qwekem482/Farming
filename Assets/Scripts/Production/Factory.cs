using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Factory : ProductionBuilding
{
    Queue<ProductData> processingQueue;
    Queue<ProductData> completeQueue;
    
    public int queueCapacity = 3;
    

    void Awake()
    {
        processingQueue = new Queue<ProductData>();
        completeQueue = new Queue<ProductData>();
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
        ReloadFactoryUIHolder();
        if (!ProductScroller.Instance.isOpen) ProductScroller.Instance.OpenScroller(this, false);
        FactoryUIHolder.Instance.gameObject.SetActive(true);
        if (state == ProductionBuildingState.Processing) TimerUI.Instance.ShowTimer(gameObject);
    }
    
    protected override void SaveState()
    {
        DateTime productCompletedTime = DateTime.Now;
        Queue<SavedProcessingData> saveProcessing = new Queue<SavedProcessingData>();
        Queue<string> saveCompleted = new Queue<string>();
        
        if(processingQueue.Count > 0)
        {
            productCompletedTime += TimeSpan.FromSeconds(gameObject.GetComponent<Timer>().TimeLeft);
            foreach(ProductData productData in processingQueue)
            {
                if (productData != processingQueue.Peek()) 
                    productCompletedTime += productData.processingTime.ConvertToTimeSpan();
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
        Queue<ProductData> savedCompleted, TimeSpan timeLeft = default)
    {
        uniqueID = factoryID;
        queueCapacity = queueCap;
        processingQueue = savedProcessing;
        completeQueue = savedCompleted;
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
        
        if (StorageSystem.Instance.IsSufficient(data.materials))
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
            EventManager.Instance.QueueEvent(new StorageItemChangeEvent(item));
        }
        
        processingQueue.Enqueue(data);
        processingCoroutine ??= StartCoroutine(ProcessingProduct());
    }
    
    protected override IEnumerator ProcessingProduct(TimeSpan timeLeft = default)
    {
        state = ProductionBuildingState.Processing;
        while (processingQueue.Count > 0)
        {
            ProductData productData = processingQueue.Peek();
            WaitForSeconds processingTime;
            if (timeLeft == default)
            {
                processingTime = new WaitForSeconds(productData.processingTime.ToSecond());
            } else
            {
                processingTime = new WaitForSeconds((float) timeLeft.TotalSeconds);
                timeLeft = default;
            }
            
            Timer.CreateTimer(gameObject, productData.product.itemName, 
                productData.processingTime, OnSkipProcessingProduct, timeLeft);

            SaveState();
            
            yield return processingTime;
            
            OnCompleteProcessingProduct();
        }
        state = ProductionBuildingState.Idle;
        processingCoroutine = null;
    }

    protected override void OnSkipProcessingProduct()
    {
        StopCoroutine(processingCoroutine);
        OnCompleteProcessingProduct();
        
        state = ProductionBuildingState.Idle;
        processingCoroutine = null;
        processingCoroutine ??= StartCoroutine(ProcessingProduct());
    }

    protected override void OnCompleteProcessingProduct()
    {
        completeQueue.Enqueue(processingQueue.Dequeue());
        SaveState();
        if (ReferenceEquals(FactoryUIHolder.Instance.currentFactory, this)) ReloadFactoryUIHolder();
    }

    public void ReloadFactoryUIHolder()
    {
        FactoryUIHolder.Instance.Init(this, processingQueue, completeQueue);
    }

    public override void Place()
    {
        base.Place();
        SaveState();
    }
}

