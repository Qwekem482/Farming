using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : SingletonMonoBehavior<TimerUI>
{
    [SerializeField] Camera mainCam;
    [SerializeField] Slider progress;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI timeLeft;
    [SerializeField] Button skipButton;
    [SerializeField] TextMeshProUGUI skipPriceText;
    
    bool countdown;
    Timer timer;

    protected override void Awake()
    {
        base.Awake();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        skipButton.onClick.AddListener(Skip);
    }

    public void ShowTimer(GameObject triggerObject)
    {
        timer = triggerObject.GetComponent<Timer>();

        if (timer == null) return;

        nameText.text = timer.TimerName;
        skipPriceText.text = timer.SkipPrice.ToString();
        skipButton.gameObject.SetActive(true);

        triggerObject.TryGetComponent(out Collider2D collid2D);
        Vector3 objectPosition = triggerObject.transform.position;
        
        if (collid2D != null)
        {
            Vector3 position = new Vector3(objectPosition.x,
                objectPosition.y - collid2D.bounds.size.y,
                objectPosition.z);
            transform.position = mainCam.WorldToScreenPoint(position);
        } else
        {
            transform.position = mainCam.WorldToScreenPoint
                (triggerObject.transform.position + Vector3.down);

        }

        countdown = true;
        FixedUpdate();
        OpenCurtain();
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    void HideTimer()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        UICurtain.Instance.TurnOff();
        timer = null;
        countdown = false;
    }
    
    void OpenCurtain()
    {
        
        UICurtain.Instance.Transparent();
        UICurtain.Instance.AssignOnClickOnce(() =>
        {
            HideTimer();
            UICurtain.Instance.TurnOff();
        });
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

    void Skip()
    {
        EventManager.Instance.AddListenerOnce<SufficientCurrencyEvent>(OnSufficientCurrency);
        EventManager.Instance.AddListenerOnce<InsufficientCurrencyEvent>(OnInsufficientCurrency);
        
        EventManager.Instance.QueueEvent(new CurrencyChangeEvent(-timer.SkipPrice, CurrencyType.Gold));
        UICurtain.Instance.TurnOff();
    }

    void OnSufficientCurrency(SufficientCurrencyEvent info)
    {
        timer.Skip();
        skipButton.gameObject.SetActive(false);
        EventManager.Instance.RemoveListener<InsufficientCurrencyEvent>(OnInsufficientCurrency);
    }

    void OnInsufficientCurrency(InsufficientCurrencyEvent info)
    {
        EventManager.Instance.RemoveListener<SufficientCurrencyEvent>(OnSufficientCurrency);
    }
}
