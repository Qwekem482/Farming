using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] Button silver;
    [SerializeField] Button exp;

    void Start()
    {
        silver.onClick.AddListener(() => GetSilver(10));
        exp.onClick.AddListener(() => GetExp(10));
    }
    public void GetSilver(int amount)
    {
        CurrencyChangeEvent coinEvent = new CurrencyChangeEvent(amount, CurrencyType.Silver);
        EventManager.Instance.QueueEvent(coinEvent);
    }

    public void GetExp(int amount)
    {
        ExpAddedEvent expEvent = new ExpAddedEvent(amount);
        EventManager.Instance.QueueEvent(expEvent);
    }
}
