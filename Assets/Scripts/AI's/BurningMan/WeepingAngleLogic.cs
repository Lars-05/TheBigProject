using UnityEngine;

public class WeepingAngleLogic : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    public bool CanMove()
    {
        return IsVisible(_playerCamera);
    }
    bool IsVisible(Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        return !GeometryUtility.TestPlanesAABB(planes, GetComponent<Renderer>().bounds);
    }
}
