using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalUIHolder : SingletonMonoBehavior<HorizontalUIHolder>
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform sicklePanel;
    
    [SerializeField] RectTransform buildingConfirmPanel;
    public Button confirmButton;
    public Button cancelButton;

    bool isOpen;
    void Start()
    {
        SetSickleViewState(false);
        gameObject.SetActive(false);
    }
    
    void SetSickleViewState(bool state)
    {
        buildingConfirmPanel.gameObject.SetActive(!state);
        sicklePanel.gameObject.SetActive(state);
    }

    public void OpenUI(bool isSickle)
    {
        if (isOpen) return;
        isOpen = true;
        SetSickleViewState(isSickle);
        if(isSickle) UICurtain.Instance.AddListener(CloseUI);
        gameObject.SetActive(true);
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y, 0.2f);
    }

    public void CloseUI()
    {
        if (!isOpen) return;
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y, 0.2f)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                isOpen = false;
            });
        UICurtain.Instance.RemoveListener(CloseUI);
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }
}
