using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] LayerMask interactableLayer;
    private IInteractable _lastInteractable = null;
    void FixedUpdate()
    {
        Vector3 dir =   camera.transform.forward;
        Debug.DrawRay(camera.transform.position, dir );
        if(Physics.Raycast(camera.transform.position, dir, out RaycastHit hit, Mathf.Infinity, interactableLayer))
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
        if (_lastInteractable != null)
        {
            _lastInteractable.OnHoverExit();
            _lastInteractable = null;
        }
    }
}
