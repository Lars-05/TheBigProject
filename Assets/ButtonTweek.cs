using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTween : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Float")]
    [SerializeField] private float floatAmount = 6f;
    [SerializeField] private float floatDuration = 2f;

    [Header("Hover")]
    [SerializeField] private float hoverScale = 1.08f;
    [SerializeField] private float hoverDuration = 0.15f;

    private RectTransform rectTransform;

    private Vector2 startPos;
    private Vector3 startScale;
    private Vector3 startRotation;

    private Tween floatTween;
    private Tween rotateTween;

    private float randomDurationMultiplier;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        startPos = rectTransform.anchoredPosition;
        startScale = rectTransform.localScale;
        startRotation = rectTransform.localEulerAngles;

        randomDurationMultiplier = Random.Range(0.85f, 1.15f);
    }

    private void Start()
    {
        float adjustedDuration = floatDuration * randomDurationMultiplier;

        floatTween = rectTransform
            .DOAnchorPosY(startPos.y + floatAmount, adjustedDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(Random.Range(0f, 1f))
            .SetUpdate(true); 

        rotateTween = rectTransform
            .DOLocalRotate(startRotation + new Vector3(0, 0, 2f), adjustedDuration * 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(Random.Range(0f, 1f))
            .SetUpdate(true); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.PlaySound("hover", transform.position, true);

        rectTransform
            .DOScale(startScale * hoverScale, hoverDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform
            .DOScale(startScale, hoverDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.PlaySound("click", transform.position, true);
    }

    private void OnDestroy()
    {
        floatTween?.Kill();
        rotateTween?.Kill();
    }
}