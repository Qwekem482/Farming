using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : SingletonMonoBehavior<OrderUI>
{
    [SerializeField] OrderSlot[] orderSlots = new OrderSlot[9];
    [SerializeField] Image[] requestItemImages = new Image[4];
    [SerializeField] TextMeshProUGUI[] requestAmountTexts = new TextMeshProUGUI[4];
    [SerializeField] Button cancelOrderButton;
    [SerializeField] Button deliveryOrderButton;
    [SerializeField] RectTransform content;

    void Start()
    {
        gameObject.SetActive(false);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    public void OpenUI()
    {
        SetAllOrderButtons();
        SetContentSize();
        orderSlots[0].button.onClick.Invoke();
        gameObject.SetActive(true);
        gameObject.transform.DOScale(1, 0.2f);
        UICurtain.Instance.AddListener(CloseUI, false);
    }

    void CloseUI()
    {
        gameObject.transform.DOScale(0, 0.2f)
            .OnComplete(() => gameObject.SetActive(false));
        UICurtain.Instance.RemoveListener(CloseUI);
    }
    
    void SetAllOrderButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            orderSlots[i].Init(i);
        }
    }

    public void SetOnClick(int index)
    {
        OnClickOrderInfo(index);
        OnClickDeliveryButton(index);
        OnClickCancelButton(index);
    }
    
    void OnClickCancelButton(int index)
    {
        cancelOrderButton.onClick.RemoveAllListeners();
        cancelOrderButton.onClick.AddListener(() =>
        {
            OrderSystem.Instance.CancelOrder(index);
            orderSlots[index].Init(index);
            Invoke(nameof(ResetInfoUI), 0.05f);
            Invoke(nameof(SetAllOrderButtons), 0.05f);
        });
    }
    
    void OnClickDeliveryButton(int index)
    {
        deliveryOrderButton.onClick.RemoveAllListeners();
        deliveryOrderButton.interactable = OrderSystem.Instance.orders[index].CanBeDelivery();
        deliveryOrderButton.onClick.AddListener(() =>
        {
            OrderSystem.Instance.DeliveryOrder(index);
            orderSlots[index].Init(index);
            Invoke(nameof(ResetInfoUI), 0.05f);
            Invoke(nameof(SetAllOrderButtons), 0.05f);
        });
    }

    void OnClickOrderInfo(int index)
    {
        Item[] requirements = OrderSystem.Instance.orders[index].requestItems;
        
        for (int i = 0; i < 4; i++)
        {
            if (i >= requirements.Length)
            {
                requestAmountTexts[i].gameObject.SetActive(false);
                requestItemImages[i].gameObject.SetActive(false);
                requestItemImages[i].preserveAspect = true;
                continue;
            }

            requestAmountTexts[i].gameObject.SetActive(true);
            requestItemImages[i].gameObject.SetActive(true);
            requestItemImages[i].sprite = requirements[i].collectible.icon;
            
            requestAmountTexts[i].text = StorageSystem.Instance.GetStoreAmount(requirements[i].collectible)
                                         + "/"
                                         + requirements[i].amount;
        }
    }

    void SetContentSize()
    {
        int activeChild = 0;
        foreach(Transform child in content) if (child.gameObject.activeSelf) activeChild++;
        Vector2 newSize = new Vector2(content.sizeDelta.x, activeChild * 165 + 20);
        content.sizeDelta = newSize;
    }
    
    void ResetInfoUI()
    {
        for (int i = 0; i < 9; i++)
        {
            if (!(OrderSystem.Instance.orders[i].resetTime != default
                  && OrderSystem.Instance.orders[i].resetTime > DateTime.Now))
            {
                orderSlots[i].button.onClick.Invoke();
                break;
            }
        }
    }
}
