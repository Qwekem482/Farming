using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "CustomObject/Collectibles/Product")]
public class ProductData : ScriptableObject
{
    public Collectible product;
    public Item[] materials;
    public TimePeriod processingTime;

    protected virtual void OnValidate()
    {
        if (materials.Length <= 4) return;
        Array.Resize(ref materials, 4);
        Debug.LogError("Material array is limited to 4 elements");
    }
}
