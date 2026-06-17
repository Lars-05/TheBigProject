using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class VFXTool
{
    
    public static Volume volume;
    
    public enum VFXType
    {
        Vignette,
        Bloom,
        ChromaticAberration,
        ColorAdjustments
    }
    
    public static VolumeComponent GetEffect(VFXType type)
    {
        if (!volume || !volume.profile)
            return null;

        switch (type)
        {
            case VFXType.Vignette:
                if (volume.profile.TryGet(out Vignette v))
                    return v;
                break;

            case VFXType.Bloom:
                if (volume.profile.TryGet(out Bloom b))
                    return b;
                break;

            case VFXType.ChromaticAberration:
                if (volume.profile.TryGet(out ChromaticAberration c))
                    return c;
                break;

            case VFXType.ColorAdjustments:
                if (volume.profile.TryGet(out ColorAdjustments ca))
                    return ca;
                break;
            default:
                Debug.LogWarning("[VFXTool]: Not accounted for VFX type: " + type);
                break;
        }

        return null;
    }
    
}