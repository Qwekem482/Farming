using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICurtain : SingletonMonoBehavior<UICurtain>, IPointerClickHandler
{
    [SerializeField] Image curtain;
    readonly Color transparent = new Color(255, 255, 255, 0);
    readonly Color darkFade = new Color(0, 0, 0, 128);
    readonly List<UnityAction> onClick = new List<UnityAction>();

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void Transparent()
    {
        TurnOn();
        curtain.color = transparent;
    }

    public void DarkFade()
    {
        TurnOn();
        curtain.color = new Color(0, 0, 0, 0.5f);
    }

    void TurnOn()
    {
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void AssignOnClickOnce(UnityAction action)
    {
        onClick.Add(action);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick.Count != 0)
        {
            foreach (UnityAction action in onClick) action.Invoke();
        }
        
        onClick.Clear();
    }
}
