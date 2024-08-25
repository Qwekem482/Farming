using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderUI : SingletonMonoBehavior<OrderUI>
{
    [SerializeField] OrderSlot[] orderSlots = new OrderSlot[9];
    [SerializeField] Image[] requestItemImages = new Image[4];
    [SerializeField] TextMeshProUGUI[] requestAmountTexts = new TextMeshProUGUI[4];
    [SerializeField] TextMeshProUGUI expReward;
    [SerializeField] TextMeshProUGUI currencyReward;
    [SerializeField] Image currencyUnit;
    [SerializeField] Button cancelOrderButton;
    [SerializeField] Button deliveryOrderButton;

    void OnEnable()
    {
        SetupAllOrderButtons();
    }


    #region SetupButton

    //Assign each active orderButton to order in system
    //Show orderButton
    void SetupAllOrderButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            SetupOrderButton(i);
        }
    }
    
    void SetupOrderButton(int index)
    {
        Order order = OrderSystem.Instance.orders[index];
        orderSlots[index].button.onClick.AddListener(() =>
        {
            Debug.Log("Setup order: #" + index);
            SetupOrderInfo(order);
            SetupDeliveryButton(order, index);
            SetupCancelButton(index);

            cancelOrderButton.interactable = true;
        });

        DateTime resetTime = order.resetTime;
        if(resetTime != default) orderSlots[index].Init(OrderSlotState.Abort, resetTime);
        else if(order.CanBeDelivery()) orderSlots[index].Init(OrderSlotState.CanBeDelivery, resetTime);
        else orderSlots[index].Init(OrderSlotState.InProgress, resetTime);
    }
    
    //Assign delivery and cancel button
    void SetupCancelButton(int index)
    {
        cancelOrderButton.onClick.RemoveAllListeners();
        cancelOrderButton.onClick.AddListener(() =>
        {
            Debug.Log("Cancel order: #" + index);
            OrderSystem.Instance.CancelOrder(index);
            orderSlots[index].SlotState = OrderSlotState.Abort;
            
            InfoOnClickCancel();
            ButtonOnClickCancel();
        });
    }

    void SetupDeliveryButton(Order order, int index)
    {
        deliveryOrderButton.onClick.RemoveAllListeners();
        deliveryOrderButton.interactable = order.CanBeDelivery();
        deliveryOrderButton.onClick.AddListener(() =>
        {
            Debug.Log("Delivery order: #" + index);
            OrderSystem.Instance.DeliveryOrder(index);
            SetupOrderButton(index);
            orderSlots[index].button.onClick.Invoke();
        });
    }

    void ButtonOnClickCancel()
    {
        deliveryOrderButton.interactable = false;
        cancelOrderButton.interactable = false;
    }

    #endregion

    #region DisplayInfo

    void SetupOrderInfo(Order order)
    {
        SetupRewardInfo(order.exp, order.silver, order.gold);
        SetupRequirement(order.requestItems);
    }
    
    //Assign data to show order reward detail
    void SetupRewardInfo(int exp, int silver, int gold)
    {
        expReward.text = exp.ToString();
        if (silver == 0)
        {
            currencyReward.text = gold.ToString();
            currencyUnit.sprite = ResourceManager.Instance.goldSprite;
        } else
        {
            currencyReward.text = silver.ToString();
            currencyUnit.sprite = ResourceManager.Instance.silverSprite;
        }
    }
    
    //Assign data to show order requirement detail
    void SetupRequirement(IReadOnlyList<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            requestAmountTexts[i].text = StorageSystem.Instance.GetStoreAmount(items[i].collectible) + "/" + items[i].amount;
            requestItemImages[i].sprite = items[i].collectible.icon;
        }
        
        ChangeArrayState(requestItemImages, 4, false);
        ChangeArrayState(requestAmountTexts, 4, false);
        ChangeArrayState(requestAmountTexts, items.Count, true);
        ChangeArrayState(requestItemImages, items.Count, true);
    }

    void InfoOnClickCancel()
    {
        ChangeArrayState(requestItemImages, 4, false);
        ChangeArrayState(requestAmountTexts, 4, false);
        currencyReward.text = "";
        expReward.text = "";
    }
    #endregion
    
    //Code to change active state of all the component that hold info
    void ChangeArrayState<T>(IReadOnlyList<T> array, int quantity, bool state) where T : Component
    {
        try
        {
            for (int i = 0; i < quantity; i++)
            {
                array[i].gameObject.SetActive(state);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Cannot setup array state, maybe quantity error:\n" + e.Message + "\n" + e.StackTrace);
            throw;
        }
    }
    
}
