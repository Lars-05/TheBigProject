
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRotater : MonoBehaviour
{
    [SerializeField] private Transform _objectTransform;
    
    private Vector2 _rotateVelocity;
    private readonly float _rotateSpeed = 100;
    private readonly float _rotateDrag = 0.05f;
    private bool _rotating;
    
    private IDisposable _clickDisposable;
    private IDisposable _clickReleaseDisposable;
    private IDisposable _moveDisposable;
    private IDisposable _exitDisposable;
    
    
    private void OnEnable()
    {
        _clickDisposable = InputManager.Instance.BindPerformed("MouseClick", OnMouseClick);
        _moveDisposable = InputManager.Instance.BindPerformed("MouseMove", OnMouseMove);
        _clickReleaseDisposable = InputManager.Instance.BindCancelled("MouseClick", OnMouseRelease);
        _clickReleaseDisposable = InputManager.Instance.BindPerformed("Exit", Exit);
    }
    
    private void OnDisable()
    {
        _clickDisposable?.Dispose();
        _moveDisposable?.Dispose();
        _clickReleaseDisposable?.Dispose();
        _exitDisposable?.Dispose();
    }

    private void FixedUpdate()
    {
        float newY = _objectTransform.localEulerAngles.y + (-_rotateVelocity.x);
        float newZ = _objectTransform.localEulerAngles.z + (-_rotateVelocity.y);
        _objectTransform.localEulerAngles = new Vector3(0, newY, newZ);

        if (_rotating) return;

        _rotateVelocity.x = Mathf.Lerp(_rotateVelocity.x, 0, _rotateDrag);
        _rotateVelocity.y = Mathf.Lerp(_rotateVelocity.y, 0, _rotateDrag);
    }

    private void OnMouseClick(InputAction.CallbackContext _)
    {
        _rotateVelocity = Vector2.zero;
        _rotating = true;
    }
    
    private void OnMouseRelease(InputAction.CallbackContext _)
    {
        _rotating = false;
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if (!_rotating)
            return;

        Vector2 mouseDelta = context.ReadValue<Vector2>();
        _rotateVelocity = SenstivityManager.mouseSensitivity * mouseDelta * _rotateSpeed * Time.deltaTime;
    }

    private void Exit(InputAction.CallbackContext _)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Instance.SwitchActionMap("Player");
        Destroy(transform.parent.gameObject);
    }
    
}
