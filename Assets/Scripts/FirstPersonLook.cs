using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 100f;
    private Vector2 _mouseDelta;
    private IDisposable _moveCameraSubscription;
    private IDisposable _moveCameraStopSubscription;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void OnEnable()
    {
        _moveCameraSubscription = InputManager.Instance.BindPerformed("MoveCamera", GetMouseInput);
        _moveCameraSubscription = InputManager.Instance.BindCancelled("MoveCamera", StopCameraMove);
    }

    private void OnDisable()
    {
        _moveCameraSubscription?.Dispose();
    }
    
    private void FixedUpdate()
    {
        Transform camera = Camera.main.transform;
        Vector3 cameraEulerAngles = camera.eulerAngles;
        Vector3 eulerAngles = transform.eulerAngles;
        
        camera.rotation =  Quaternion.Euler(
            cameraEulerAngles.x + -_mouseDelta.y * (_sensitivity * 2) * Time.fixedDeltaTime,
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
