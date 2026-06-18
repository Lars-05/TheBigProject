using UnityEngine;

public static class VFXManager
{
    private static InterlacingShaderEffect interlacingShaderEffect;
    private static VignettePulsingEffect vignetteController;

    public static void Init(
        InterlacingShaderEffect interlacing,
        VignettePulsingEffect vignette)
    {
        interlacingShaderEffect = interlacing;
        vignetteController = vignette;
    }

    public static void SetInterlacingStrength(float strength, float duration)
    {
        interlacingShaderEffect.SetIntensity(strength, duration);
    }
    public static void ResetInterlacingStrength()
    {
        interlacingShaderEffect.ResetIntensity();
    }

    public static void SetFov(float fov, float duration)
    {
        FovController.SetFov(fov, duration);
    }

    public static void StartVignettePulse()
    {
        Debug.Log("Starting Vignette Pulse");
        vignetteController.StartPulsing();
    }

    public static void StopVignettePulse()
    {
        vignetteController.StopPulsing();
    }
    
    public static void StartScreenShake(float magnitude, float roughness, float fadeInTime)
    {
        CameraShakeManager.StartCameraShake(magnitude,  roughness,  fadeInTime);
    }
    
    public static void StopScreenShake(float fadeOutTime)
    {
        CameraShakeManager.StopCameraShake(fadeOutTime);
    }
    
    public static void OneShotScreenShake(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
        CameraShakeManager.ShakeCameraOneShot(magnitude, roughness, fadeInTime, fadeOutTime);
    }


    public static void SetVignettePulsingParameters(
        float minIntensity,
        float maxIntensity,
        float duration)
    {
        vignetteController?.SetPulsingParameters(
            minIntensity,
            maxIntensity,
            duration);
    }
}