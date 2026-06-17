using EZCameraShake;
using UnityEngine;

static public class CameraShakeManager
{
    private static CameraShakeInstance currentCameraShake; 
    
    static void ShakeCameraOneShot(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
        currentCameraShake = CameraShaker.Instance.ShakeOnce(magnitude, roughness, fadeInTime, fadeOutTime);
    }
    static void StartCameraShake(float magnitude, float roughness, float fadeInTime)
    {
        currentCameraShake = CameraShaker.Instance.StartShake(4, 4, 1);
    }
    static void VoidStopCameraShake(float fadeOutTime)
    {
        currentCameraShake.StartFadeOut(fadeOutTime);
    }
}
