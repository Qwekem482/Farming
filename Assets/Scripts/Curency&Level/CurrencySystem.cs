using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class CurrencySystem : SingletonMonoBehavior<CurrencySystem>, IGameSystem
{
    readonly Dictionary<CurrencyType, int> currencyAmounts = new Dictionary<CurrencyType, int>();
    
    [SerializedDictionary("Type", "Text")]
    [SerializeField]SerializedDictionary<CurrencyType, TextMeshProUGUI> currencyTexts = new SerializedDictionary<CurrencyType, TextMeshProUGUI>();
    
    protected override void Awake()
    {
        base.Awake();
    }

    public void StartingSystem()
    {
        EventManager.Instance.AddListener<CurrencyChangeEvent>(OnCurrencyChange);
        EventManager.Instance.AddListener<InsufficientCurrencyEvent>(OnInsufficient);
        EventManager.Instance.AddListener<SufficientCurrencyEvent>(OnSufficient);
    }

    public void LoadCurrency(int silver, int gold)
    {
        currencyAmounts.Add(CurrencyType.Silver, silver);
        currencyAmounts.Add(CurrencyType.Gold, gold);
        ChangeCurrencyText(CurrencyType.Silver);
        ChangeCurrencyText(CurrencyType.Gold);
    }

    void OnCurrencyChange(CurrencyChangeEvent info)
    {
        if (info.amount <= 0)
        {
            if (currencyAmounts[info.currencyType] < Math.Abs(info.amount))
            {
                EventManager.Instance.QueueEvent(new InsufficientCurrencyEvent(info.amount, info.currencyType));
                return;
            }
        }
        
        EventManager.Instance.QueueEvent(new SufficientCurrencyEvent(info.amount, info.currencyType));
    }

    void OnInsufficient(InsufficientCurrencyEvent info)
    {
        Debug.Log("Insufficient"); //show a windows dialog
    }

    void OnSufficient(SufficientCurrencyEvent info)
    {
        currencyAmounts[info.currencyType] += info.amount;
        ChangeCurrencyText(info.currencyType);
        EventManager.Instance.QueueEvent(new SaveCurrencyEvent(
                currencyAmounts[CurrencyType.Silver], 
                currencyAmounts[CurrencyType.Gold]));
    }

    void ChangeCurrencyText(CurrencyType type)
    {
        currencyTexts[type].text = currencyAmounts[type].ToString();
    }
    
}

public enum CurrencyType
{
    Silver,
    Gold,
}