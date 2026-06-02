using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TarrotCardDisplayer : MonoBehaviour
{
    public static bool IsCardOpen = false;
    [SerializeField] private Image _backgroundBlur;
    [SerializeField] private Image _cardSpriteRenderer;
    private Sprite _card;
    
    public void OnBackgroundClicked()
    {
        if(!IsCardOpen)
            return;
        CloseCard();
    }
    
    public void DisplayCard(Sprite card)
    {
        if (IsCardOpen)
            CloseCard();
        
        _cardSpriteRenderer.sprite = _card;
        _cardSpriteRenderer.gameObject.SetActive(true);
        IsCardOpen = true;
    }

    private void CloseCard()
    {
        _cardSpriteRenderer.gameObject.SetActive(false);
        IsCardOpen = false;
        
    }
}
