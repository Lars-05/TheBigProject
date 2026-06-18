using EZCameraShake;
using UnityEngine;

static public class CameraShakeManager
{
    private static CameraShakeInstance currentCameraShake; 
    
    public static void ShakeCameraOneShot(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
        currentCameraShake = CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
    }
    public static void StartCameraShake(float magnitude, float roughness, float fadeInTime)
    {
        currentCameraShake = CameraShaker.Instance.StartShake(4, 4, 1);
    }
    public static void StopCameraShake(float fadeOutTime)
    {
        currentCameraShake.StartFadeOut(fadeOutTime);
    }
}
