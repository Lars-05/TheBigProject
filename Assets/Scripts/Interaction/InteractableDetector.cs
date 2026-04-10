using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _interactableLayer;
    private IInteractable _lastInteractable = null;
    
    private void FixedUpdate()
    {
        Vector3 dir = _camera.transform.forward;
        Debug.DrawRay(_camera.transform.position, dir );
        if(Physics.Raycast(_camera.transform.position, dir, out RaycastHit hit, Mathf.Infinity, _interactableLayer))
        {
            GameObject obj = hit.collider.gameObject;
            if (obj.TryGetComponent(out IInteractable interactable))
            {
                if (_lastInteractable == null || _lastInteractable != interactable)
                {
                    _lastInteractable = interactable;
                    _lastInteractable.OnHoverEnter();
                }
                _lastInteractable.OnHoverStay();
                return;
            }
        }

        if (_lastInteractable == null)
            return;

        _lastInteractable.OnHoverExit();
        _lastInteractable = null;
    }
}
