using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : SingletonMonoBehavior<OrderUI>
{
    [SerializeField] Button[] showOrderButtons = new Button[9];
    [SerializeField] TextMeshProUGUI[] resetOrderText = new TextMeshProUGUI[9];
    [SerializeField] Image[] showOrderImage = new Image[9];
    [SerializeField] Image[] requestItemImages = new Image[4];
    [SerializeField] TextMeshProUGUI[] requestAmountTexts = new TextMeshProUGUI[4];
    [SerializeField] Button cancelOrderButton;
    [SerializeField] Button devileryOrderButton;
    
    [SerializeField] Sprite pendingOrderSprite;
    [SerializeField] Sprite readyOrderSprite;

    Order currentOrder;
    int ordersQuantity;
    int requestItemsQuantity;

    public void Init()
    {
        GetGeneralInfo();
        SetupArrayState();
    }

    #region Private Method

    void GetGeneralInfo()
    {
        try
        {
            ordersQuantity = OrderSystem.Instance.orderQuantity;
            currentOrder = OrderSystem.Instance.orderList[0];
            requestItemsQuantity = OrderSystem.Instance.orderList[0].requestItems.Length;
        }
        catch (Exception e)
        {
            Debug.Log("Cannot get general info:\n" + e.Message + "\n" + e.StackTrace);
            throw;
        }
    }

    void SetupArrayState()
    {
        ChangeArrayState(showOrderButtons, 9, false);
        ChangeArrayState(requestItemImages, 4, false);
        ChangeArrayState(requestAmountTexts, 4, false);
            
        ChangeArrayState(showOrderButtons, ordersQuantity, true);
        ChangeArrayState(requestItemImages, requestItemsQuantity, true);
        ChangeArrayState(requestAmountTexts, requestItemsQuantity, true);
        
        Debug.Log("ordersQuantity: " + ordersQuantity + "\n" +
                  "requestItemsQuantity: " + requestItemsQuantity);
    }

    void SetupFirstOrderInfo()
    {
        try
        {
            
        }
        catch (Exception e)
        {
            Debug.Log("Cannot setup array state, maybe currentOrder = null:\n" + e.Message + "\n" + e.StackTrace);
            throw;
        }
    }

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

    #endregion
}
