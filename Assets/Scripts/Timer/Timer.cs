using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    public string TimerName { get; private set; }
    bool IsRunning { get; set; }
    DateTime startTime;
    DateTime finishTime;
    public TimeSpan duration;
    public UnityEvent onComplete;
    public UnityEvent onSkip;
    public double TimeLeft { get; private set; }
    public int SkipPrice { get; private set; }

    void InitTimer(string processName, DateTime start, TimeSpan timerDuration)
    {
        TimerName = processName;
        startTime = start;
        duration = timerDuration;
        finishTime = start.Add(timerDuration);
        onComplete = new UnityEvent();
        onComplete.AddListener(() =>
        {
            UICurtain.Instance.InvokeAndClose();
            Destroy(this);
        });
        onSkip = new UnityEvent();
        onSkip.AddListener(onComplete.Invoke);
    }

    void TimerBegin(TimeSpan timeLeft)
    {
        TimeLeft = timeLeft.TotalSeconds;
        IsRunning = true;
    }

    void FixedUpdate()
    {
        if (!IsRunning) return;
        if (TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
            SkipPrice = TimeLeft switch
            {
                0 => 0,
                < 60 => 1,
                > 60 => (int)(TimeLeft / 60) + 1,
                _ => SkipPrice
            };
        } else
        {
            onComplete.Invoke();
            TimeLeft = 0;
            IsRunning = false;
        }
    }

    public string TimeLeftString()
    {
        TimeSpan time = TimeSpan.FromSeconds(TimeLeft);
        return time.Hours.ToString("00") + ":" +
               time.Minutes.ToString("00") + ":" +
               time.Seconds.ToString("00");
    }
    public void Skip()
    {
        TimeLeft = 0;
        finishTime = DateTime.Now;
        onSkip.Invoke();
    }

    public static Timer CreateTimer(GameObject source, string processName, TimePeriod period,
        UnityAction onCompleteEvent, UnityAction onSkipEvent = null, TimeSpan timeLeft = default)
    {
        Timer timer = source.AddComponent<Timer>();
        timer.InitTimer(processName, DateTime.Now, period.ConvertToTimeSpan());
        timer.TimerBegin(timeLeft == default ? period.ConvertToTimeSpan() : timeLeft);
        timer.onComplete.AddListener(() => onCompleteEvent?.Invoke());
        timer.onSkip.AddListener(() => onSkipEvent?.Invoke());
        return timer;
    }
}

[Serializable]
public class TimePeriod
{
    public int days = 0;
    public int hours = 0;
    public int minutes = 0;
    public int seconds = 0;

    public TimeSpan ConvertToTimeSpan()
    {
        return new TimeSpan(days, hours, 
            minutes, seconds);
    }

    public float ToSecond()
    {
        return (float) ConvertToTimeSpan().TotalSeconds;
    }

    public string ConvertToStringWithoutDay()
    {
        return ConvertToTimeSpan().ToString(@"hh\:mm\:ss");
    }
    
    
}
