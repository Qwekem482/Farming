using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

//Component
public class StorageCell : RegularCell
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI amount;
    
    public void AssignData(Collectible collectible, int itemAmount)
    {
        icon.sprite = collectible.icon;
        amount.text = itemAmount.ToString();
    }
}
