using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICurtain : SingletonMonoBehavior<UICurtain>, IPointerClickHandler
{
    [SerializeField] Image curtain;
    public readonly UnityEvent onClick = new UnityEvent();

    void Start()
    {
        gameObject.SetActive(false);
    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Invoke();
    }

    public void AddListener(UnityAction call, bool isTransparent = true)
    {
        onClick.AddListener(call);
        gameObject.SetActive(true);
        curtain.color = isTransparent ? Color.clear : new Color(0, 0, 0, 0.5f);
    }

    public void RemoveListener(UnityAction call)
    {
        onClick.RemoveListener(call);
        gameObject.SetActive(false);
    }

    public void Invoke()
    {
        onClick.Invoke();
        TurnOff();
    }
}
