using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderSlot : MonoBehaviour
{
    public Button button;
    public Image image;
    public TextMeshProUGUI resetTimeText;
    public int index;
    
    OrderSlotState state;
    DateTime resetTime;

    public OrderSlotState SlotState
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            switch (value)
            {
                case OrderSlotState.InProgress:
                    OnInProgress();
                    break;
                case OrderSlotState. CanBeDelivery:
                    OnCanBeDelivery();
                    break;
                case OrderSlotState.Abort:
                    OnAbort();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }

    public void Init(OrderSlotState slotState, DateTime time)
    {
        resetTime = time;
        Debug.Log("State: " + slotState);
        SlotState = slotState;
    }

    void OnInProgress()
    {
        image.color = Color.white;
        button.interactable = true;
        resetTimeText.gameObject.SetActive(false);
    }

    void OnCanBeDelivery()
    {
        image.color = Color.green;
        button.interactable = true;
        resetTimeText.gameObject.SetActive(false);
    }

    void OnAbort()
    {
        image.color = Color.clear;
        button.interactable = false;

        //resetTimeText.text = (resetTime - DateTime.Now).ToString(@"hh\:mm\:ss");
        InvokeRepeating(nameof(Countdown), 0, 1);
        resetTimeText.gameObject.SetActive(true);
        //resetTimeText.text = resetTime.ToString("hh:mm:ss");
    }

    void Countdown()
    {
        resetTimeText.text = (resetTime - DateTime.Now).ToString(@"hh\:mm\:ss");
        if (DateTime.Now < resetTime) return;
        
        CancelInvoke(nameof(Countdown));
        
        Init(OrderSystem.Instance.orders[index].CanBeDelivery()
            ? OrderSlotState.CanBeDelivery
            : OrderSlotState.InProgress, default);
    }
}

public enum OrderSlotState
{
    InProgress,
    CanBeDelivery,
    Abort,
}
