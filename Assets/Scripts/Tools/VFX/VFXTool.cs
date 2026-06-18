using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class VFXTool
{
    public static Volume volume;
    public static T GetEffect<T>() where T : VolumeComponent
    {
        if (volume == null || volume.profile == null)
            return null;

        volume.profile.TryGet(out T effect);
        return effect;
    }
    
}