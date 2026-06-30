using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System;

public class Parchment : MonoBehaviour, IInteractable
{
    
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] Animator anM;
    float timer, tBase = 15;

    private Material[] _defaultMaterials;
    private Material[] _selectedMaterials;

    private bool _collected;
    bool parchP = false;
    private IDisposable _sub;

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<MeshRenderer>();

        if (_renderer == null)
        {
            Debug.LogError($"No MeshRenderer found on {name}");
            return;
        }
        
        _defaultMaterials = _renderer.materials;
        
        _selectedMaterials = new Material[_defaultMaterials.Length + 1];

        for (int i = 0; i < _defaultMaterials.Length; i++)
            _selectedMaterials[i] = _defaultMaterials[i];

        _selectedMaterials[^1] = _selectedMaterial;
    }

    void Update()
    {
        if(timer <= 0 && parchP)
        {
            
            anM.SetBool("parchP", false); 
            parchP = !parchP;
        }
        timer -= Time.deltaTime;
    }

    public void OnHoverEnter()
    {
        if (_renderer != null)
            _renderer.materials = _selectedMaterials;
    }

    public void OnHoverExit()
    {
        if (_renderer != null)
            _renderer.materials = _defaultMaterials;
    }

    public void OnHoverStay()
    {
        OnHoverEnter();
    }

    public void OnInteract()
    {
        ParchmentPop();
    }

    void ParchmentPop()
    {
        if (parchP)
        {   
            anM.SetBool("parchP", false); 
            parchP = !parchP;
        }
        else
        {
            anM.SetBool("parchP", true); 
            timer =+ tBase;
            parchP = !parchP;
        }
    }
}
