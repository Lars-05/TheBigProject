using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(Volume))]

public class VFXBootstrap : MonoBehaviour
{
    private Volume volume;
    private void Awake()
    {
        Debug.Log("[VFXBootstrap] Awake");
        VFXTool.volume = GetComponent<Volume>();
    }
}

