using TMPro;
using UnityEngine;

public class RelicUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private int _relicsNeeded;
    private void OnEnable()
    {
        _text = GetComponent<TextMeshProUGUI>();
        Relic.OnRelicAdded.AddListener(IncrementRelicText);
        IncrementRelicText(0);
    }
    
    private void IncrementRelicText(int amount)
    {
        _text.text = $" Relics Collected: {amount.ToString()}/{RelicManager.relicsNeeded}";
    }
}