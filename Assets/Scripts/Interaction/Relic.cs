using System;
using UnityEngine;
using UnityEngine.Events;

public class Relic : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private GameObject _rotateObject;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _interactionDistance;

    private Material[] _defaultMaterials;
    private Material[] _selectedMaterials;

    private bool _collected;

    public static UnityEvent<int> OnRelicAdded = new();
    private static int _relicCount;

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

    private bool PlayerInRange()
    {
        return Vector3.Distance(_player.transform.position, _renderer.bounds.center) < _interactionDistance;
    }

    public void OnHoverEnter()
    {
        if (_renderer != null && PlayerInRange())
            _renderer.materials = _selectedMaterials;
    }

    public void OnHoverExit()
    {
        if (_renderer != null)
            _renderer.materials = _defaultMaterials;
    }

    public void OnHoverStay()
    {
        if(PlayerInRange())
        {
            OnHoverEnter();
        }
    }

    public void OnInteract()
    {
        if (!PlayerInRange())
            return;
            
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        InputManager.Instance.SwitchActionMap("ObjectView");

        if (_rotateObject != null)
            _rotateObject.SetActive(true);
        
        if (!_collected)
        {
            _collected = true;
            _relicCount++;
            OnRelicAdded.Invoke(_relicCount);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _interactionDistance);
    }
}