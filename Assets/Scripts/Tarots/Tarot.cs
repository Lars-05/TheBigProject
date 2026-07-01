using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class Tarot : MonoBehaviour, IUsable
{
    [SerializeField] protected float _duration = 0.1f;
    
    public abstract void Use(GameObject player);
    
    protected void StartCooldownAnimation()
    {
        Image image = GetComponent<Image>();
        image.fillAmount = 1;
        image.DOFillAmount(0, _duration);
        Destroy(gameObject, _duration + 0.2f);
    }
}
