using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessingSlot : MonoBehaviour
{
    protected ProductData productData;
    [SerializeField] Image icon;

    public void Init(ProductData data)
    {
        productData = data;
        icon.color = Color.white;
        icon.sprite = data.product.icon;
    }

    public void Hide()
    {
        productData = null;
        icon.color = Color.clear;
    }

    public void UnloadAllData()
    {
        productData = null;
        icon.sprite = null;
        icon.color = Color.white;
    }
}
