using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalUIHolder : SingletonMonoBehavior<HorizontalUIHolder>
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform sicklePanel;
    
    [SerializeField] RectTransform buildingConfirmPanel;
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;

    bool isOpen;
    void Start()
    {
        confirmButton.onClick.AddListener(BuildingSystem.Instance.ConfirmBuild);
        cancelButton.onClick.AddListener(BuildingSystem.Instance.CancelBuild);
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
        gameObject.SetActive(true);
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y, 0.2f);
        UICurtain.Instance.onClick.AddListener(CloseUI);
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
        UICurtain.Instance.onClick.RemoveListener(CloseUI);
    }
}
