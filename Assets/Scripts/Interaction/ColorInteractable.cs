using UnityEngine;

public class DebugInteractible : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _selectedMaterial;

    private MeshRenderer _renderer;
    private Material[] _selectedMaterials;
    private Material[] _defaultMaterials;

    private void Awake()
    {
       
        _renderer = GetComponent<MeshRenderer>();
       
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
        Debug.Log("Hovering...");
    }

    private int i = 0;
    public void OnInteract()
    {
        if (_renderer == null)
            return;
        i++;
        if (i == 3)
        {
            Destroy(gameObject);
        }
    }
}