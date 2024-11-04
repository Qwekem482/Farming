using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : SingletonMonoBehavior<SettingUI>
{
    [SerializeField] Button settingButton;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider soundVolume;
    [SerializeField] Button quitGame;

    void Start()
    {
        musicVolume.onValueChanged.AddListener(_ => 
            AudioManager.Instance.ChangeBackgroundVolume(musicVolume.value));
        soundVolume.onValueChanged.AddListener(_ => 
            AudioManager.Instance.ChangeEffectVolume(soundVolume.value));
        settingButton.onClick.AddListener(OpenSetting);
        quitGame.onClick.AddListener(Application.Quit);
        
        gameObject.SetActive(false);
        transform.localScale = new Vector3(0, 0, 0);
    }

    void OpenSetting()
    {
        gameObject.SetActive(true);
        transform.DOScale(1, 0.2f)
            .SetEase(Ease.OutBack);
        UICurtain.Instance.AddListener(CloseSetting, false);
    }

    void CloseSetting()
    {
        UICurtain.Instance.RemoveListener(CloseSetting);
        transform.DOScale(0, 0.2f)
            .OnComplete(() => gameObject.SetActive(false))
            .SetEase(Ease.InBack);
    }
}
