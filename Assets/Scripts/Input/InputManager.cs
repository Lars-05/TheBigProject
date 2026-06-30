using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput _playerInput;

    private void Awake()
    {
        Debug.Log($"[InputManager] Awake ({GetInstanceID()})");

        if (Instance != null && Instance != this)
        {
            Debug.Log("[InputManager] Duplicate destroyed.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput == null)
        {
            Debug.LogError("[InputManager] No PlayerInput component found!");
            return;
        }

        Debug.Log($"[InputManager] Default Map: {_playerInput.currentActionMap?.name}");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnableInput(true);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[InputManager] Scene Loaded: {scene.name}");

        if (_playerInput == null)
            _playerInput = GetComponent<PlayerInput>();

        EnableInput(true);

        Debug.Log($"[InputManager] Current Map: {_playerInput.currentActionMap?.name}");
    }

    public IDisposable BindPerformed(string actionName, Action<InputAction.CallbackContext> callback)
    {
        InputAction action = GetAction(actionName);

        if (action == null)
            return null;

        Debug.Log($"[InputManager] Bind Performed -> {actionName}");

        action.performed += callback;

        return new InputSubscription(() =>
        {
            Debug.Log($"[InputManager] Unbind Performed -> {actionName}");
            action.performed -= callback;
        });
    }

    public IDisposable BindStarted(string actionName, Action<InputAction.CallbackContext> callback)
    {
        InputAction action = GetAction(actionName);

        if (action == null)
            return null;

        action.started += callback;

        return new InputSubscription(() =>
        {
            action.started -= callback;
        });
    }

    public IDisposable BindCancelled(string actionName, Action<InputAction.CallbackContext> callback)
    {
        InputAction action = GetAction(actionName);

        if (action == null)
            return null;

        action.canceled += callback;

        return new InputSubscription(() =>
        {
            action.canceled -= callback;
        });
    }

    public void EnableInput(bool value)
    {
        if (_playerInput == null)
            return;

        if (value)
        {
            _playerInput.ActivateInput();
            _playerInput.currentActionMap?.Enable();

            Debug.Log("[InputManager] Input Enabled");
        }
        else
        {
            _playerInput.DeactivateInput();
            Debug.Log("[InputManager] Input Disabled");
        }
    }

    public void SwitchActionMap(string actionMapName)
    {
        InputActionMap map = _playerInput.actions.FindActionMap(actionMapName);

        if (map == null)
        {
            Debug.LogError($"ActionMap '{actionMapName}' not found.");
            return;
        }

        Debug.Log($"[InputManager] Switching to {actionMapName}");

        _playerInput.SwitchCurrentActionMap(actionMapName);
    }

    private InputAction GetAction(string actionName)
    {
        InputAction action = _playerInput.actions.FindAction(actionName);

        if (action == null)
            Debug.LogError($"[InputManager] Action '{actionName}' not found.");

        return action;
    }
}