using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class OpenScroll : MonoBehaviour
{
    [SerializeField] private RectTransform imageRect;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.OutBack;

    public  UnityEvent OnScrollClose;
    private float closedScaleX;

    private void Awake()
    {
        for (int i = 0; i < imageRect.childCount; i++)
        {
            imageRect.GetChild(i).gameObject.SetActive(false);
        }
        closedScaleX = 0f;
        imageRect.localScale = new Vector3(0f, 1f, 1f);
    }

    public void Open()
    {
        AudioManager.PlaySound("PageTurn");
        imageRect.DOScaleX(1f, duration)
            .SetEase(ease);
        for (int i = 0; i < imageRect.childCount; i++)
        {
            imageRect.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        imageRect.DOScaleX(closedScaleX, duration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                for (int i = 0; i < imageRect.childCount; i++)
                {
                    imageRect.GetChild(i).gameObject.SetActive(false);
                }

                OnScrollClose?.Invoke();
            });
    }
}