using System;
using UnityEngine;

using System;
using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
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
       _renderer.materials = _selectedMaterials;
    }

    public void OnHoverExit()
    {
        _renderer.materials = _defaultMaterials;
    }

    public void OnHoverStay()
    {
        Debug.Log("Hovering...");
    }

    public void OnInteract()
    {
    }
}
