using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorToggle : MonoBehaviour
{
    private IDisposable _toggleCursorSubscription;
    
    private void OnEnable()
    {
        _toggleCursorSubscription = InputManager.Instance.BindPerformed("ToggleCursor", ToggleCursor);
    }

    private void OnDisable()
    {
        _toggleCursorSubscription.Dispose();
    }

    private void ToggleCursor(InputAction.CallbackContext _)
    {
        Cursor.visible = !Cursor.visible;

        Cursor.lockState = Cursor.lockState switch {
            CursorLockMode.Locked => CursorLockMode.None,
            CursorLockMode.None => CursorLockMode.Locked,
            _ => Cursor.lockState
        };
    }
}
