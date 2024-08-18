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

    protected override void OnValidate()
    {
        FixInput();
        FixID();

        outputValue = inputPrice + (int)processingTime.ToSecond() / 30;
    }
    protected override void FixID()
    {
        if (char.IsDigit(id[0])) id = "C" + id;
    }
    protected override void FixInput()
    {
        if (inputPrice > 0) return;
        inputPrice = 1;
    }
}