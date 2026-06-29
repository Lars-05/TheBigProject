using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class Tarot : MonoBehaviour, IUsable
{
    [SerializeField] protected float _duration = 0.1f;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    public abstract void Use(GameObject player);
    
    protected void StartCooldownAnimation()
    {
        _image.fillAmount = 1;
        _image.DOFillAmount(0, _duration);
        Destroy(gameObject, _duration + 0.2f);
    }
}
