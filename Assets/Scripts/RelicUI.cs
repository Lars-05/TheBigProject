using TMPro;
using UnityEngine;

public class RelicUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private void OnEnable()
    {
        _text = GetComponent<TextMeshProUGUI>();
        Relic.OnRelicAdded.AddListener(IncrementRelicText);
    }
    
    private void IncrementRelicText(int amount)
    {
        _text.text = amount.ToString();
    }
}