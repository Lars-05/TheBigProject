using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class VFXManager : MonoBehaviour
{
    [Header("Shader")]
    [SerializeField] private Material interlacingMaterial;

    [Header("Volume")]
    [SerializeField] private Volume volume;
    private Vignette vignette;

    void Awake()
    {
    
        Set(interlacingMaterial, "_Intensity", 1f, 1f);
    }
 
    public Tween SetFloat(Material mat, string property, float target, float duration)
        => Set(mat, property, target, duration, false);

    public Tween SetInt(Material mat, string property, int target, float duration)
        => Set(mat, property, target, duration, true);
    
    
    public Tween Set(Material mat, string property, float target, float duration, bool isInt = false)
    {
        if (!mat) return null;

        int id = Shader.PropertyToID(property);
        float start = mat.GetFloat(id);

        return DOTween.To(
            () => start,
            x =>
            {
                start = x;

                if (isInt)
                    mat.SetInt(id, Mathf.RoundToInt(x));
                else
                    mat.SetFloat(id, x);
            },
            target,
            duration
        );
    }

}