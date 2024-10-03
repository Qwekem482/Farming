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
    [SerializeField] Image clockIcon;
    [SerializeField] Image background;

    bool isUnlocked;

    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }

        set
        {
            isUnlocked = value;
            ChangeCellState(value);
        }
    }

    public void AssignData(ProductData data, Canvas canvas, Factory currentFactory)
    {
        for (int i = 0; i < 4; i++)
        {
            if (data.materials.Length > i)
            {
                materials.ElementAt(i).Key.sprite = data.materials[i].collectible.icon;
                int storageAmount = StorageSystem.Instance.GetStoreAmount(data.materials[i].collectible);
                materials.ElementAt(i).Value.text = storageAmount + "/" + data.materials[i].amount;
            } else
            {
                materials.ElementAt(i).Key.gameObject.SetActive(false);
            }
        }

        processTime.text = data.processingTime.ConvertToStringWithoutDay();
        ProductUI product = productIcon.gameObject.AddComponent<ProductUI>();
        product.Init(canvas, data, currentFactory, this);
        
        IsUnlocked = LevelSystem.Instance.currentLevel >= data.level;
    }
    
    public void AssignData(CropData data, Canvas canvas, Sprite coin)
    {
        for (int i = 1; i < 4; i++)
        {
            materials.ElementAt(i).Key.gameObject.SetActive(false);
        }

        materials.ElementAt(0).Key.sprite = coin;
        materials.ElementAt(0).Value.text = data.inputPrice.ToString();
        
        processTime.text = data.processingTime.ConvertToStringWithoutDay();
        CropUI crop = productIcon.gameObject.AddComponent<CropUI>();
        crop.Init(canvas, data, this);

        IsUnlocked = LevelSystem.Instance.currentLevel >= data.level;
    }

    void ChangeCellState(bool state)
    {
        Color cellColor = state ? Color.white : Color.gray;

        foreach(KeyValuePair<Image,TextMeshProUGUI> material in materials)
        {
            material.Key.color = cellColor;
            material.Value.color = cellColor;
        }

        processTime.color = cellColor;
        productIcon.color = cellColor;
        clockIcon.color = cellColor;
        background.color = cellColor;
        gameObject.GetComponent<ProductUI>().enabled = state;
    }
}
