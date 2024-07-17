using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "CustomObject/Collectibles/Crop")]
public class CropData : ProductData
{
    public int price;
    public Sprite processingSprite;
    public Sprite completeSprite;

    protected override void OnValidate()
    {
        if (materials.Length <= 4) return;
        Array.Resize(ref materials, 4);
        Debug.LogError("Material array is limited to 4 elements");
    }
}