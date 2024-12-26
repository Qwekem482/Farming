using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : SingletonMonoBehavior<AudioManager>
{
    [SerializeField] AudioSource backgroundAudio;
    [SerializeField] AudioSource effectAudio;
    [SerializeField] List<AudioClip> backgroundClip;
    [SerializeField] List<AudioClip> effectClip;

    public float BackgroundVolume { get; private set; }
    public float EffectVolume { get; private set; }

    public void PlayBackgroundClip(int clipIndex)
    {
        backgroundAudio.DOFade(0, 0.5f)
            .OnComplete(() =>
            {
                backgroundAudio.clip = backgroundClip[clipIndex];
                backgroundAudio.Play();
                backgroundAudio.DOFade(BackgroundVolume, 0.5f);
            });
    }

    public void PlayEffectClip(int clipIndex)
    {
        if (effectAudio.isPlaying) return;
        effectAudio.clip = effectClip[clipIndex];
        effectAudio.Play();
    }

    public void ChangeBackgroundVolume(float volume)
    {
        BackgroundVolume = volume;
        backgroundAudio.volume = volume;
    }

    public void ChangeEffectVolume(float volume)
    {
        EffectVolume = volume;
        effectAudio.volume = volume;
    }
}
