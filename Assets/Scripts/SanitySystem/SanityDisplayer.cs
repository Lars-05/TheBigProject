using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplayer : MonoBehaviour
{
    [SerializeField] private Slider _sanitySlider;
    [SerializeField] private float _sliderChangeDuration;

    private int _currentSanity;
    private Tween _sliderTween;

    private void Start()
    {
        _currentSanity = (int)_sanitySlider.value;
    }

    public void IncreaseSanity(int amount)
    {
        _currentSanity -= amount;
        UpdateSlider();
    }

    public void DecreaseSanity(int amount)
    {
        _currentSanity += amount;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        _sliderTween?.Kill();

        _sliderTween = _sanitySlider.DOValue(
            _currentSanity,
            _sliderChangeDuration
        );
    }
}