using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

public class OpenScroll : MonoBehaviour
{
    [SerializeField] private RectTransform imageRect;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.OutBack;

    public UnityEvent OnScrollClose;

    private Tween currentTween;
    private bool isInitialized;

    private float closedScaleX = 0f;

    private void Awake()
    {
        if (imageRect == null)
            imageRect = GetComponent<RectTransform>();

        KillTweenImmediate();
        ResetState();
    }

    private void OnEnable()
    {
        StartCoroutine(InitAndOpen());
    }

    private IEnumerator InitAndOpen()
    {
        yield return null;

        if (this == null || imageRect == null)
            yield break;

        KillTweenImmediate();
        ResetState();

        Open();

        isInitialized = true;
    }

    private void ResetState()
    {
        if (imageRect == null) return;

        imageRect.localScale = new Vector3(0f, 1f, 1f);

        for (int i = 0; i < imageRect.childCount; i++)
        {
            imageRect.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        if (imageRect == null) return;

        KillTweenImmediate();

        AudioManager.PlaySound("PageTurn");

        for (int i = 0; i < imageRect.childCount; i++)
        {
            imageRect.GetChild(i).gameObject.SetActive(true);
        }

        currentTween = imageRect.DOScaleX(1f, duration)
            .SetEase(ease)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void Close()
    {
        if (imageRect == null) return;

        KillTweenImmediate();

        currentTween = imageRect.DOScaleX(closedScaleX, duration)
            .SetEase(Ease.InBack)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
            .OnComplete(() =>
            {
                if (imageRect == null) return;

                for (int i = 0; i < imageRect.childCount; i++)
                {
                    imageRect.GetChild(i).gameObject.SetActive(false);
                }

                OnScrollClose?.Invoke();
            });
    }

    private void KillTweenImmediate()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        currentTween = null;
    }

    private void OnDisable()
    {
        KillTweenImmediate();
    }

    private void OnDestroy()
    {
        KillTweenImmediate();
    }
}