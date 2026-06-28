using DG.Tweening;
using UnityEngine;

public class TextTween : MonoBehaviour
{
    [Header("Float")]
    [SerializeField] private float floatAmount = 6f;
    [SerializeField] private float floatDuration = 2f;

    private RectTransform rectTransform;

    private Vector2 startPos;
    private Vector3 startRotation;

    private Tween floatTween;
    private Tween rotateTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        startPos = rectTransform.anchoredPosition;
        startRotation = rectTransform.localEulerAngles;
    }

    private void Start()
    {
        floatTween = rectTransform
            .DOAnchorPosY(startPos.y + floatAmount, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); // Ignore Time.timeScale

        rotateTween = rectTransform
            .DOLocalRotate(startRotation + new Vector3(0, 0, 2f), floatDuration * 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); // Ignore Time.timeScale
    }

    private void OnDestroy()
    {
        floatTween?.Kill();
        rotateTween?.Kill();
    }
}