using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICurtain : SingletonMonoBehavior<UICurtain>, IPointerClickHandler
{
    [SerializeField] Image curtain;
    readonly UnityEvent onClick = new UnityEvent();
    int listenerCount = 0;

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
        listenerCount++;
        gameObject.SetActive(true);
        curtain.color = isTransparent ? Color.clear : new Color(0, 0, 0, 0.5f);
    }

    public void RemoveListener(UnityAction call)
    {
        onClick.RemoveListener(call);
        listenerCount--;
        if (listenerCount == 0) TurnOff();
    }

    void Invoke()
    {
        onClick.Invoke();
        onClick.RemoveAllListeners();
        listenerCount = 0;
        TurnOff();
    }
}
