using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class Tarot : MonoBehaviour, IUsable
{
    [SerializeField] protected float _cooldown;
    [SerializeField] private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    
    public abstract void Use(GameObject player);
    
    protected void StartCooldownAnimation()
    {
        _image.fillAmount = 0;
        _image.DOFillAmount(1f, _cooldown);
    }
}
