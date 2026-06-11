using UnityEngine;
using UnityEngine.Events;

public class Relic : MonoBehaviour, IInteractable 
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private GameObject _rotateObject;
    [SerializeField] private MeshRenderer _renderer;
    
    private Material[] _selectedMaterials;
    private Material[] _defaultMaterials;

    public static UnityEvent OnRelicAdded;
    
    private void Awake()
    {
        _defaultMaterials = new Material[1];
        _selectedMaterials = new Material[2];
        
        _defaultMaterials[0] = _defaultMaterial;
        _selectedMaterials [0] = _defaultMaterial;
        _selectedMaterials[1] = _selectedMaterial;

    }

    public void OnHoverEnter()
    {
        if (_renderer == null)
            return;
        _renderer.materials = _selectedMaterials;
    }

    public void OnHoverExit()
    {
        if (_renderer == null)
            return;
        _renderer.materials = _defaultMaterials;
    }

    public void OnHoverStay()
    {
        if (_renderer == null)
            return;
    }
    
    public void OnInteract()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InputManager.Instance.SwitchActionMap("ObjectView");
        _rotateObject.SetActive(true);
        OnRelicAdded.Invoke();
    }
}
