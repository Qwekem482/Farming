using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "CustomObject/Collectibles/Crop")]
public class CropData : ProductionOutputData
{
    public int price;
    public Sprite processingSprite;
    public Sprite completeSprite;

    protected override void OnValidate()
    {
        FixInput();
        FixID();
    }
    protected override void FixID()
    {
        if (char.IsDigit(id[0])) id = "C" + id;
    }
    protected override void FixInput()
    {
        if (price > 0) return;
        price = 1;
    }
}