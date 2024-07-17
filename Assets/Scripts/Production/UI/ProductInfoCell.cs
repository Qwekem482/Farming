using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

public class ProductInfoCell : RegularCell
{
    [SerializeField] SerializedDictionary<Image, TextMeshProUGUI> materials;
    [SerializeField] TextMeshProUGUI processTime;
    [SerializeField] Image productIcon;

    public void AssignData(ProductData data, Canvas canvas, Factory currentFactory)
    {
        for (int i = 0; i < 4; i++)
        {
            if (data.materials.Length > i)
            {
                materials.ElementAt(i).Key.sprite = data.materials[i].collectible.icon;
                int storageAmount = StorageSystem.Instance.GetCollectibleStoreAmount(data.materials[i].collectible);
                materials.ElementAt(i).Value.text = storageAmount + "/" + data.materials[i].amount;
            } else
            {
                materials.ElementAt(i).Key.gameObject.SetActive(false);
            }
        }

        processTime.text = data.processingTime.ConvertToStringWithoutDay();
        ProductUI product = productIcon.gameObject.AddComponent<ProductUI>();
        product.Init(canvas, data, currentFactory, this);
    }
    
    public void AssignData(CropData data, Canvas canvas, Sprite coin)
    {
        for (int i = 1; i < 4; i++)
        {
            materials.ElementAt(i).Key.gameObject.SetActive(false);
        }

        materials.ElementAt(0).Key.sprite = coin;
        materials.ElementAt(0).Value.text = data.price.ToString();
        
        processTime.text = data.processingTime.ConvertToStringWithoutDay();
        CropUI crop = productIcon.gameObject.AddComponent<CropUI>();
        crop.Init(canvas, data, this);
    }
}
