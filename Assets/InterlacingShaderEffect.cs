using DG.Tweening;
using UnityEngine;

public class InterlacingShaderEffect : MonoBehaviour
{
    [Header("Shader")]
    [SerializeField] private Material interlacingMaterial;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float defaultIntensity = 1f;
    public void ResetIntensity()
    {
        interlacingMaterial.SetFloat("_Intensity", defaultIntensity);
    }
    public void SetIntensity(float targetIntensity, float duration)
    {
        interlacingMaterial.DOKill(); 

        interlacingMaterial.DOFloat(targetIntensity, "_Intensity", duration);
    }
}