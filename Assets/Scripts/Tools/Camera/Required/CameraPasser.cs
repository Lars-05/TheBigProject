using UnityEngine;

public class CameraPasser : MonoBehaviour
{
    void Awake()
    {
        FovController.Register(GetComponent<Camera>());
    }
}