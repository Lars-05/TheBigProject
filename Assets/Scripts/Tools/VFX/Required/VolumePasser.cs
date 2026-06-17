using UnityEngine;
using UnityEngine.Rendering;

public class VolumePasser : MonoBehaviour
{
    [SerializeField] private Volume volume;
    void Awake()
    {
        VFXTool.volume = volume;
    }
}
