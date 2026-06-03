
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    
    [SerializeField] private float _frequency;
    [SerializeField] private float _amplitude;
    [SerializeField] private float _smooth;

    private float _velocityMultiplier;
    private float _bobTime;
    [SerializeField] private float _extraVelocity = 1;

    private Vector3 _pos = Vector3.zero;
    private void FixedUpdate()
    {
        _velocityMultiplier = Mathf.Lerp(_velocityMultiplier, _rb.linearVelocity.magnitude, 1 * Time.fixedDeltaTime);

        _pos.y = Mathf.Lerp(_pos.y, Mathf.Sin(Time.time * _frequency) * _amplitude * 1.4f, _smooth * Time.fixedDeltaTime);
        _pos.x = Mathf.Lerp(_pos.x, Mathf.Cos(Time.time * _frequency / 2f) * _amplitude * 1.6f, _smooth * Time.fixedDeltaTime);
        transform.localPosition = _pos;
    }
}
