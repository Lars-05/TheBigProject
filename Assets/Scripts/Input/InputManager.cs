using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerInput _playerInput;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        
        _playerInput = GetComponent<PlayerInput>();
    }

    public IDisposable BindPerformed(string actionName, Action<InputAction.CallbackContext> callback)
    {
        InputAction inputAction = GetAction(actionName);
        inputAction.performed += callback;
        
        return new InputSubscription(() =>
        {
            inputAction.performed -= callback;
        });
    }
    
    public IDisposable BindStarted(string actionName, Action<InputAction.CallbackContext> callback)
    {
        InputAction inputAction = GetAction(actionName);
        inputAction.started += callback;
        
        return new InputSubscription(() =>
        {
            inputAction.started -= callback;
        });
    }
    
    public IDisposable BindCancelled(string actionName, Action<InputAction.CallbackContext> callback)
    {
        InputAction inputAction = GetAction(actionName);
        inputAction.canceled += callback;
        
        return new InputSubscription(() =>
        {
            inputAction.canceled -= callback;
        });
    }
    
    public void EnableInput(bool value)
    {
        if(value)
            _playerInput.ActivateInput();
        else
            _playerInput.DeactivateInput();
    }
    
    private InputAction GetAction(string actionName)
    {
        return _playerInput.actions[actionName];
    }
    
}
