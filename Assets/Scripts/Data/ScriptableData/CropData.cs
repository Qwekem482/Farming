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
    
}