using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Crop", menuName = "CustomObject/Collectibles/Crop")]
public class CropData : ProductionOutputData
{
    public int inputPrice;
    public Sprite processingSprite;
    public Sprite completeSprite;
    
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        FixID();

        inputPrice = level;
        outputValue = inputPrice + (int)processingTime.ToSecond() / 60;
    }
    protected override void FixID()
    {
        if (char.IsDigit(productID[0]) || !productID[0].Equals('P')) productID = "P" + productID;
    }
    
#endif
}