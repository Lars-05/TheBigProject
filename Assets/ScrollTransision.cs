using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ScrollTransision : MonoBehaviour
{
    [SerializeField] private RectTransform imageRect;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float openScaleX = 1f;

    public UnityEvent OnScrollClose;

    public bool IsOpen { get; private set; }

    private const float closedScaleX = 0f;

    private Tween currentTween;

    private void Awake()
    {
        if (imageRect == null)
            imageRect = GetComponent<RectTransform>();

        KillTween();
        ResetToClosedInstant();
    }

    private void Start()
    {
        StartCoroutine(AutoCloseAtStart());
    }

    private IEnumerator AutoCloseAtStart()
    {
        yield return null;
        yield return Close();
    }

    private void ResetToClosedInstant()
    {
        imageRect.localScale = new Vector3(closedScaleX, 1f, 1f);

        for (int i = 0; i < imageRect.childCount; i++)
            imageRect.GetChild(i).gameObject.SetActive(false);

        IsOpen = false;
    }

    public IEnumerator Open()
    {
        KillTween();

        gameObject.SetActive(true);
        AudioManager.PlaySound("PageTurn");

        for (int i = 0; i < imageRect.childCount; i++)
            imageRect.GetChild(i).gameObject.SetActive(true);

        imageRect.localScale = new Vector3(closedScaleX, 1f, 1f);

        IsOpen = false;

        bool done = false;

        currentTween = imageRect.DOScaleX(openScaleX, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .OnComplete(() => done = true);

        yield return new WaitUntil(() => done);

        IsOpen = true;
    }

    public IEnumerator Close()
    {
        KillTween();

        gameObject.SetActive(true);
        AudioManager.PlaySound("PageTurn");
        for (int i = 0; i < imageRect.childCount; i++)
            imageRect.GetChild(i).gameObject.SetActive(true);

        imageRect.localScale = new Vector3(openScaleX, 1f, 1f);

        IsOpen = false;

        bool done = false;

        currentTween = imageRect.DOScaleX(closedScaleX, duration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => done = true);

        yield return new WaitUntil(() => done);

        for (int i = 0; i < imageRect.childCount; i++)
            imageRect.GetChild(i).gameObject.SetActive(false);

        OnScrollClose?.Invoke();
    }

    private void KillTween()
    {
        currentTween?.Kill();
        currentTween = null;
    }

    private void OnDisable()
    {
        KillTween();
    }

    private void OnDestroy()
    {
        KillTween();
    }
}