using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Factory : MonoBehaviour
{
    Queue<ProductData> processingQueue;
    Queue<ProductData> completeQueue;
    
    public FactoryType type;
    public FactoryState state;
    public int queueCapacity = 3;

    protected Coroutine processingCoroutine;

    void Awake()
    {
        processingQueue = new Queue<ProductData>();
        completeQueue = new Queue<ProductData>();
    }
    void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        ReloadFactoryUIHolder();
        if (!ProductScroller.Instance.isOpen) ProductScroller.Instance.OpenScroller(this);
        FactoryUIHolder.Instance.gameObject.SetActive(true);
        if (state == FactoryState.Processing) TimerSystem.Instance.ShowTimer(gameObject);
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
            
            Timer.CreateTimer(gameObject, productData.product.itemName, productData.processingTime, OnSkipProcessingProduct);
            
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
        if (ReferenceEquals(FactoryUIHolder.Instance.currentFactory, this)) ReloadFactoryUIHolder();
    }

    public void ReloadFactoryUIHolder()
    {
        FactoryUIHolder.Instance.Init(this, processingQueue, completeQueue);
    }

    /*void OnSufficient(SufficientItemsEvent info)
    {
        EventManager.Instance.RemoveListener<InsufficientItemsEvent>(OnInsufficient);
    }

    void OnInsufficient(InsufficientItemsEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientItemsEvent>(OnSufficient);
    }*/
}

public enum FactoryState
{
    Idle,
    Processing,
    Complete,
}

public enum FactoryType
{
    Grinder,
    SteamStation,
    Field,
}
