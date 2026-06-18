using EZCameraShake;
using UnityEngine;

public static class CameraShakeManager
{
    private static CameraShakeInstance currentShake;

    public static void ShakeCameraOneShot(
        float magnitude,
        float roughness,
        float fadeInTime,
        float fadeOutTime)
    {
        if (CameraShaker.Instance == null)
        {
            Debug.LogError("CameraShaker.Instance is null!");
            return;
        }

        currentShake = CameraShaker.Instance.ShakeOnce(
            magnitude,
            roughness,
            fadeInTime,
            fadeOutTime
        );
    }

    public static void StartCameraShake(
        float magnitude,
        float roughness,
        float fadeInTime)
    {
        if (CameraShaker.Instance == null)
        {
            Debug.LogError("CameraShaker.Instance is null!");
            return;
        }
        
        StopCameraShake(0f);

        currentShake = CameraShaker.Instance.StartShake(
            magnitude,
            roughness,
            fadeInTime
        );

        if (currentShake != null)
        {
            currentShake.DeleteOnInactive = true;
        }
    }

    public static void StopCameraShake(float fadeOutTime)
    {
        StopImmediately();
    }
    
    public static void StopImmediately()
    {
        if (currentShake == null)
            return;

        currentShake.ScaleMagnitude = 0f;
        currentShake.StartFadeOut(0f);
        currentShake = null;
    }
}