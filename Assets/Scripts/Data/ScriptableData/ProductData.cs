using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "CustomObject/Product")]
public class ProductData : ProductionOutputData
{
    public Item[] materials;

    protected override void OnValidate()
    {
        FixInput();
        FixID();
    }

    protected override void FixID()
    {
        if (char.IsDigit(id[0])) id = "P" + id;
    }

    protected override void FixInput()
    {
        if (materials.Length <= 4) return;
        Array.Resize(ref materials, 4);
        Debug.LogError("Material array is limited to 4 elements");
    }

    public SavedProcessingData ConvertToSavedData()
    {
        DateTime completedDateTime = DateTime.Now + processingTime.ConvertToTimeSpan();
        return new SavedProcessingData(id, completedDateTime);
    }
    
}
