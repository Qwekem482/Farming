using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimerUI : SingletonMonoBehavior<TimerUI>
{
    [SerializeField] Camera mainCam;
    [SerializeField] Slider progress;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI timeLeft;
    [SerializeField] Button skipButton;
    [SerializeField] TextMeshProUGUI skipPriceText;
    [SerializeField] RectTransform rectTransform;
    
    bool countdown;
    Timer timer;

    protected override void Awake()
    {
        base.Awake();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        skipButton.onClick.AddListener(SkipButtonAction);
    }

    public void ShowTimer(GameObject triggerObject)
    {
        timer = triggerObject.GetComponent<Timer>();

        if (timer == null) HideTimer();

        nameText.text = timer.TimerName;
        skipPriceText.text = timer.SkipPrice.ToString();
        skipButton.gameObject.SetActive(true);

        triggerObject.TryGetComponent(out Collider2D collid2D);
        
        if (collid2D != null)
        {
            Bounds bound = collid2D.bounds;
            Vector3 bottomPos = new Vector3(bound.center.x, bound.min.y, bound.center.z);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(bottomPos);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform, 
                screenPos, 
                null, 
                out Vector2 uiPos
            );

            rectTransform.anchoredPosition = uiPos;
        } else
        {
            transform.position = mainCam.WorldToScreenPoint
                (triggerObject.transform.position + Vector3.down);
        }

        countdown = true;
        FixedUpdate();
        UICurtain.Instance.AddListener(HideTimer);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    void HideTimer()
    {
        UICurtain.Instance.RemoveListener(HideTimer);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        timer = null;
        countdown = false;
    }

    void FixedUpdate()
    {
        if (!countdown) return;
        
        if(timer.TimeLeft == 0)
        {
            HideTimer();
            return;
        }
        
        progress.value = (float)(1 - timer.TimeLeft / timer.duration.TotalSeconds);
        timeLeft.text = timer.TimeLeftString();
    }

    void SkipButtonAction()
    {
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientCurrency);
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-timer.SkipPrice, CurrencyType.Gold));
    }

    void OnSufficientCurrency(SufficientCurrencyEvent info)
    {
        timer.Skip();
        skipButton.gameObject.SetActive(false);
        UICurtain.Instance.RemoveListener(HideTimer);
        EventManager.Instance.RemoveListener<InsufficientCurrencyEvent>(OnInsufficientCurrency);
    }

    void OnInsufficientCurrency(InsufficientCurrencyEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientCurrencyEvent>(OnSufficientCurrency);
    }
}
