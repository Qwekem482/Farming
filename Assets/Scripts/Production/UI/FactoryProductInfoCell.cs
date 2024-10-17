
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryProductInfoCell : ProductInfoCell
{
    [SerializeField] SerializedDictionary<Image, TextMeshProUGUI> materials;
    
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
        IsUnlocked = LevelSystem.Instance.currentLevel >= data.level;
        
        ProductUI product = productIcon.gameObject.AddComponent<ProductUI>();
        product.Init(canvas, data, currentFactory, this);
    }
    
    protected override void ChangeCellState(bool state)
    {
        base.ChangeCellState(state);
        Color cellColor = state ? Color.white : Color.gray;

        foreach(KeyValuePair<Image,TextMeshProUGUI> material in materials)
        {
            material.Key.color = cellColor;
            material.Value.color = cellColor;
        }
    }
}
