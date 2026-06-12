using DG.Tweening;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Rendering;


public class VFXManager : MonoBehaviour
{
    [SerializeField] private Material interLacingShader;
    [SerializeField] private Volume volume;
    private UnityEngine.Rendering.Universal.Vignette vignette;

    void Awake()
    {
        for (int i = 0; i < volume.profile.components.Count; i++)
        {
            if(volume.profile.components[i].name == "Vignette")
            {
                UnityEngine.Rendering.Universal.Vignette vignette = (UnityEngine.Rendering.Universal.Vignette)volume.profile.components[i]; // WHY? why, why must it work like this
                                                                                                                                      // unity documentation IS SO ASSSSSS
            }
        }
    }
    
    
    
    void LerpToValue(Material mat, float value, string param, float lerpDuration)
    {
        if (Shader.Find(param))
        {
            Debug.LogError("Cannot find shader");
        }
        float progress = 0;
        float elapsedTime = 0;
        float targetValue = 0;
        float myValue = mat.GetFloat(param);

        
        while (elapsedTime < 1)
        {
            Mathf.Lerp(0, value ,progress);
            elapsedTime += Time.deltaTime / lerpDuration;
        }
        mat.SetFloat(param, targetValue );

    }
}
