using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CassetteIconPulseEffect : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Image targetImage;

    [Header("Colors")]
    [SerializeField] private Color colorA = Color.white;
    [SerializeField] private Color colorB = Color.red;

    [Header("Pulse Scale")]
    [SerializeField] private float pulseScale = 1.08f;
    [SerializeField] private float duration = 0.5f;

    [Header("Settings")]
    [SerializeField] private bool loop = true;

    private Tween colorTween;
    private Tween scaleTween;

    private Vector3 baseScale;
    private bool isActive;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        baseScale = transform.localScale;

        targetImage.color = colorA;
    }

    public void OnEnable()
    {
        EventBus.OnCassetteStart += EnablePulse;
        EventBus.OnCassetteStop += DisablePulse;
    }

    public void OnDisable()
    {
        EventBus.OnCassetteStart -= EnablePulse;
        EventBus.OnCassetteStop -= DisablePulse;
        KillTweens();
        transform.localScale = baseScale;
    }
    
    public void EnablePulse()
    {
        if (isActive) return;
        isActive = true;

        KillTweens();

       
        colorTween = targetImage
            .DOColor(colorB, duration)
            .SetLoops(loop ? -1 : 1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

      
        scaleTween = transform
            .DOScale(baseScale * pulseScale, duration)
            .SetLoops(loop ? -1 : 1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }


    public void DisablePulse()
    {
        if (!isActive) return;
        isActive = false;

        KillTweens();
        
        targetImage.DOColor(colorA, 0.15f);
        transform.DOScale(baseScale, 0.15f);
    }

    private void KillTweens()
    {
        colorTween?.Kill();
        scaleTween?.Kill();
    }
}