using System;
using UnityEngine;

[RequireComponent(typeof(InterlacingShaderEffect))]
[RequireComponent(typeof(VignettePulsingEffect))]
public class VFXManagerBootstrap : MonoBehaviour
{
    
    private void Awake()
    {
        // this is why i should be supervised by stan 24/7
        InterlacingShaderEffect interlacingShaderEffect = GetComponent<InterlacingShaderEffect>();
        VignettePulsingEffect vignetteController = GetComponent<VignettePulsingEffect>();
        VFXManager.Init(interlacingShaderEffect , vignetteController);
    }
}
