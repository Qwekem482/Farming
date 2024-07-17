using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencySystem : SingletonMonoBehavior<CurrencySystem>
{
    private static Dictionary<CurrencyType, int> currencyAmounts = new Dictionary<CurrencyType, int>();
    private static Dictionary<CurrencyType, TextMeshProUGUI> currencyTexts = new Dictionary<CurrencyType, TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> texts;
    
    new void Awake()
    {
        base.Awake();
        currencyAmounts.Add(CurrencyType.Silver, 10); //import from player's save
        currencyAmounts.Add(CurrencyType.Gold, 10); //import from player's save

        for (int i = 0; i < texts.Count; i++) 
        {
            currencyTexts.Add((CurrencyType)(i % 2), texts[i]);
            currencyTexts[(CurrencyType)(i % 2)].text = currencyAmounts[(CurrencyType)(i % 2)].ToString();
        }
    }

    void Start()
    {
        EventManager.Instance.AddListener<CurrencyChangeEvent>(OnCurrencyChange);
        EventManager.Instance.AddListener<InsufficientCurrencyEvent>(OnInsufficient);
    }

    void OnCurrencyChange(CurrencyChangeEvent info)
    {
        if (info.amount < 0)
        {
            if (currencyAmounts[info.currencyType] < Math.Abs(info.amount))
            {
                EventManager.Instance.QueueEvent(new InsufficientCurrencyEvent(info.amount, info.currencyType));
                return;
            }
            
            EventManager.Instance.QueueEvent(new SufficientCurrencyEvent(info.amount, info.currencyType));
        }

        
        currencyAmounts[info.currencyType] += info.amount;
        currencyTexts[info.currencyType].text = currencyAmounts[info.currencyType].ToString();
    }

    void OnInsufficient(InsufficientCurrencyEvent info)
    {
        Debug.Log("Insufficient"); //show a windows dialog
    }

}

public enum CurrencyType
{
    Silver,
    Gold,
}