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

    private Vector3 baseScale;
    private bool isActive;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        baseScale = transform.localScale;
        targetImage.color = colorA;
    }

    private void OnEnable()
    {
        EventBus.OnCassetteStart += EnablePulse;
        EventBus.OnCassetteStop += DisablePulse;
    }

    private void OnDisable()
    {
        EventBus.OnCassetteStart -= EnablePulse;
        EventBus.OnCassetteStop -= DisablePulse;

        targetImage.DOKill();
        transform.DOKill();

        targetImage.color = colorA;
        transform.localScale = baseScale;

        isActive = false;
    }

    public void EnablePulse()
    {
        if (isActive)
            return;

        isActive = true;

        targetImage.DOKill();
        transform.DOKill();

        targetImage
            .DOColor(colorB, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(loop ? -1 : 1, LoopType.Yoyo)
            .SetUpdate(true);

        transform
            .DOScale(baseScale * pulseScale, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(loop ? -1 : 1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    public void DisablePulse()
    {
        if (!isActive)
            return;

        isActive = false;

        targetImage.DOKill();
        transform.DOKill();

        targetImage
            .DOColor(colorA, 0.15f)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);

        transform
            .DOScale(baseScale, 0.15f)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }
}