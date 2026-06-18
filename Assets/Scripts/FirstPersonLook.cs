using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 100f;
    private Vector2 _mouseDelta;
    private Transform _cameraTransform;
    private float _pitch;
    
    private IDisposable _moveCameraSubscription;
    private IDisposable _moveCameraStopSubscription;

    private void Awake() =>
        _cameraTransform = Camera.main.transform;
    
    private void Start() =>
        Cursor.lockState = CursorLockMode.Locked;

    private void OnEnable()
    {
        _moveCameraSubscription = InputManager.Instance.BindPerformed("MoveCamera", GetMouseInput);
        _moveCameraSubscription = InputManager.Instance.BindCancelled("MoveCamera", StopCameraMove);
    }

    private void OnDisable() =>
        _moveCameraSubscription?.Dispose();
    
    private void FixedUpdate()
    {
        Vector3 cameraEulerAngles = _cameraTransform.eulerAngles;
        Vector3 eulerAngles = transform.eulerAngles;
        
        _pitch += -_mouseDelta.y * _sensitivity * Time.fixedDeltaTime;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);
        
        _cameraTransform.rotation =  Quaternion.Euler(
            _pitch,
            cameraEulerAngles.y,
            cameraEulerAngles.z);
        
        transform.rotation = Quaternion.Euler(
            eulerAngles.x,
            eulerAngles.y + _mouseDelta.x * _sensitivity * Time.fixedDeltaTime,
            eulerAngles.z);
    }

    private void GetMouseInput(InputAction.CallbackContext context) =>
        _mouseDelta = context.ReadValue<Vector2>();
    
    private void StopCameraMove(InputAction.CallbackContext _) =>
        _mouseDelta =  Vector2.zero;
}
