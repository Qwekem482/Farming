using TMPro;
using UnityEngine;


public class FieldProductInfoCell : ProductInfoCell
{
    [SerializeField] TextMeshProUGUI price;
    
    public void AssignData(CropData data, Canvas canvas)
    {
        price.text = data.inputPrice.ToString();
        processTime.text = data.processingTime.ConvertToStringWithoutDay();
        IsUnlocked = LevelSystem.Instance.currentLevel >= data.level;
        
        CropUI crop = productIcon.gameObject.AddComponent<CropUI>();
        crop.Init(canvas, data, this);
    }
}
