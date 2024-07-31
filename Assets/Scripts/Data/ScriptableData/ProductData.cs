using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "CustomObject/Product")]
public class ProductData : ScriptableObject
{
    public string id;
    public Collectible product;
    public Item[] materials;
    public TimePeriod processingTime;

    protected virtual void OnValidate()
    {
        FixMaterial();
        FixID();
    }

    void FixID()
    {
        if (char.IsDigit(id[0])) id = "P" + id;
    }

    void FixMaterial()
    {
        if (materials.Length <= 4) return;
        Array.Resize(ref materials, 4);
        Debug.LogError("Material array is limited to 4 elements");
    }

    public SavedProcessingData ConvertToSavedData()
    {
        DateTime completedTime = DateTime.Now + processingTime.ConvertToTimeSpan();
        return new SavedProcessingData(id, completedTime);
    }

    public static ProductData ConvertFromSavedData(SavedProcessingData data)
    {
        return ResourceManager.Instance.allProductData[data.productDataID];
    }
    
}
