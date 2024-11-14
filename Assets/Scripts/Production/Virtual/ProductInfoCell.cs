using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnlimitedScrollUI;

public class ProductInfoCell : RegularCell
{
    [SerializeField] protected TextMeshProUGUI processTime;
    [SerializeField] protected Image productIcon;
    [SerializeField] protected Image clockIcon;
    [SerializeField] protected Image background;

    protected bool isUnlocked;

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

    protected virtual void ChangeCellState(bool state)
    {
        Color cellColor = state ? Color.white : Color.gray;

        processTime.color = cellColor;
        productIcon.color = cellColor;
        clockIcon.color = cellColor;
        background.color = cellColor;
    }
}
