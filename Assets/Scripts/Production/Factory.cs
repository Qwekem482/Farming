using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Factory : MovableBuilding
{
    Queue<ProductData> processingQueue;
    Queue<ProductData> completeQueue;
    
    public FactoryState state;
    public int queueCapacity = 3;

    protected Coroutine processingCoroutine;

    void Awake()
    {
        processingQueue = new Queue<ProductData>();
        completeQueue = new Queue<ProductData>();
    }
    
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ReloadFactoryUIHolder();
        if (!ProductScroller.Instance.isOpen) ProductScroller.Instance.OpenScroller(this, false);
        FactoryUIHolder.Instance.gameObject.SetActive(true);
        if (state == FactoryState.Processing) TimerUI.Instance.ShowTimer(gameObject);
    }

    public override void Init(BuildingData data)
    {
        base.Init(data);
        uniqueID = SaveData.GenerateUniqueID();
        if(GetType() == typeof(Factory)) SaveFactoryState();
    }

    public void AddProcessingProduct(ProductData data)
    {
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
    
    IEnumerator ProcessingProduct()
    {
        state = FactoryState.Processing;
        while (processingQueue.Count > 0)
        {
            ProductData productData = processingQueue.Peek();
            WaitForSeconds processingTime = new WaitForSeconds(productData.processingTime.ToSecond());
            
            Timer.CreateTimer(gameObject, productData.product.itemName, 
                productData.processingTime, OnSkipProcessingProduct);

            SaveFactoryState();
            
            yield return processingTime;
            
            OnCompleteProcessingProduct(processingQueue.Dequeue());
        }
        state = FactoryState.Idle;
        processingCoroutine = null;
    }

    void OnSkipProcessingProduct()
    {
        StopCoroutine(processingCoroutine);
        OnCompleteProcessingProduct(processingQueue.Dequeue());
        
        state = FactoryState.Idle;
        processingCoroutine = null;
        processingCoroutine ??= StartCoroutine(ProcessingProduct());
    }

    void OnCompleteProcessingProduct(ProductData data)
    {
        completeQueue.Enqueue(data);
        SaveFactoryState();
        if (ReferenceEquals(FactoryUIHolder.Instance.currentFactory, this)) ReloadFactoryUIHolder();
    }

    public void ReloadFactoryUIHolder()
    {
        FactoryUIHolder.Instance.Init(this, processingQueue, completeQueue);
    }

    protected void SaveFactoryState()
    {
        EventManager.Instance.QueueEvent(new FactoryDataEvent
            (uniqueID, buildingData.id, transform.position, 
                buildingArea, processingQueue, completeQueue));
    }

    public override void Place()
    {
        base.Place();
        SaveFactoryState();
    }

    public virtual void LoadFactory(string factoryID, Queue<ProductData> savedProcessing, Queue<ProductData> savedCompleted)
    {
        uniqueID = factoryID;
        processingQueue = savedProcessing;
        completeQueue = savedCompleted;
        processingCoroutine = null;
        processingCoroutine ??= StartCoroutine(ProcessingProduct());
    }
}

public enum FactoryState
{
    Idle,
    Processing,
    Complete,
}

