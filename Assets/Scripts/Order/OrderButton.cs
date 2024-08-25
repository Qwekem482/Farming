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
            if (state == value) return;
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
        SlotState = slotState;
        resetTime = time;
    }

    void OnInProgress()
    {
        image.color = Color.white;
        resetTimeText.gameObject.SetActive(false);
    }

    void OnCanBeDelivery()
    {
        image.color = Color.green;
        resetTimeText.gameObject.SetActive(false);
    }

    void OnAbort()
    {
        image.color = Color.clear;
        button.interactable = false;
        
        resetTime = DateTime.Now + TimeSpan.FromMinutes(15);
        resetTimeText.gameObject.SetActive(true);
        resetTimeText.text = resetTime.ToString("hh:mm:ss");
    }
}

public enum OrderSlotState
{
    InProgress,
    CanBeDelivery,
    Abort,
}
