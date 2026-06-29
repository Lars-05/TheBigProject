using UnityEngine;

public class TarotPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _uiPrefab;
    private Transform _uiParent;

    private void Awake()
    {
        _uiParent = GameObject.Find("TarotParent").transform;
    }
    
    public void OnHoverEnter()
    {
        
    }
    public void OnHoverExit()
    {
        
    }
    public void OnHoverStay()
    {
        
    }

    public void OnInteract()
    {
        Instantiate(_uiPrefab, _uiParent);
        Destroy(gameObject);
    }
}
