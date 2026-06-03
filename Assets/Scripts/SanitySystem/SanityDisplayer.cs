using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SanityDisplayer : MonoBehaviour
{
    [SerializeField] private Slider _sanitySlider;
    [SerializeField] private float _sliderChangeDuration;
    private Tween _sliderTween;
    public void UpdateSlider(float pSanity)
    {
        _sliderTween?.Kill();

        _sliderTween = _sanitySlider.DOValue(
            pSanity,
            _sliderChangeDuration
        );
    }
}