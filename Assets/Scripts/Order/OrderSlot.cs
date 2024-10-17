using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderSlot : MonoBehaviour
{
    public Button button;
    
    [SerializeField] Image background;
    [SerializeField] Image currency;
    [SerializeField] TextMeshProUGUI currencyAmount;
    [SerializeField] TextMeshProUGUI expAmount;
    [SerializeField] Image[] requestIcon = new Image[4];

    int orderIndex;

    public void Init(int index)
    {
        Order order = OrderSystem.Instance.orders[index];
        
        if (order.resetTime != default && order.resetTime > DateTime.Now) gameObject.SetActive(false);
        else if (order.CanBeDelivery()) background.color = new Color(0, 1, 0, 0.75f);
        else background.color = new Color(1, 1, 1, 0.75f);

        if (order.gold != 0)
        {
            currency.sprite = ResourceManager.Instance.goldSprite;
            currencyAmount.text = order.gold.ToString();
        }
        else
        {
            currency.sprite = ResourceManager.Instance.silverSprite;
            currencyAmount.text = order.silver.ToString();
        }

        for (int i = 0; i < 4; i++)
        {
            if (order.requestItems[i] == null)
            {
                requestIcon[i].gameObject.SetActive(false);
                continue;
            }

            requestIcon[i].gameObject.SetActive(true);
            requestIcon[i].sprite = order.requestItems[i].collectible.icon;
        }
        
        expAmount.text = order.exp.ToString();
        orderIndex = index;
        button.onClick.AddListener(() => OnClick(orderIndex));
    }

    void OnClick(int index)
    {
        OrderUI.Instance.SetOnClick(index);
    }
}

