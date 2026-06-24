
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private float _sprintSpeedMultiplier = 1.25f;
    [SerializeField] private int _moveSpeed = 5;

    private bool _sprinting;

    private Vector2 _moveVelocity;
    
    private Rigidbody _rigidbody;
    
    private IDisposable _walkSubscription;
    private IDisposable _stopMoveSubscription;
    private IDisposable _startSprintSubscription;
    private IDisposable _endSprintSubscription;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _walkSubscription = InputManager.Instance.BindPerformed("Move", Move);
        _stopMoveSubscription = InputManager.Instance.BindCancelled("Move", StopMove);
        _startSprintSubscription = InputManager.Instance.BindPerformed("Sprint", StartSprint);
        _endSprintSubscription = InputManager.Instance.BindCancelled("Sprint", EndSprint);
    }

    private void OnDisable()
    {
        _walkSubscription?.Dispose();
        _stopMoveSubscription?.Dispose();
        _startSprintSubscription?.Dispose();
        _endSprintSubscription?.Dispose();
    }

    private void FixedUpdate()
    {
        if(SanityManager.isDead)
            return;
        
        Vector3 newVelocity = Vector3.zero;
        
        Transform camera = Camera.main.transform;
        newVelocity += transform.forward * (_moveVelocity.y * _moveSpeed * Time.fixedDeltaTime);
        newVelocity += camera.right * (_moveVelocity.x * _moveSpeed * Time.fixedDeltaTime);
        
        if(_sprinting)
            newVelocity *= _sprintSpeedMultiplier;
        
        _rigidbody.linearVelocity = newVelocity;
    }

    private void Move(InputAction.CallbackContext context) =>
        _moveVelocity = context.ReadValue<Vector2>();

    private void StopMove(InputAction.CallbackContext _) =>
        _moveVelocity = Vector2.zero;

    private void StartSprint(InputAction.CallbackContext _) =>
        _sprinting = true;
    
    private void EndSprint(InputAction.CallbackContext _) =>
        _sprinting = false;
}