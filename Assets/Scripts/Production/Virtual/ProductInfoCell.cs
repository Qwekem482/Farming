using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

public class ProductInfoCell : RegularCell
{
    [SerializeField] protected TextMeshProUGUI processTime;
    [SerializeField] protected Image productIcon;
    [SerializeField] Image levelLock;
    [SerializeField] protected TextMeshProUGUI levelKey;

    bool isUnlocked;

    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }

        protected set
        {
            isUnlocked = value;
            levelLock.gameObject.SetActive(!value);
        }
    }
    
}
