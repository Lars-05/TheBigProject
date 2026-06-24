using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System.Collections;

public class VignettePulsingEffect : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField, Range(0f, 1f)] private float minIntensity = 0.1f;
    [SerializeField, Range(0f, 1f)] private float maxIntensity = 0.4f;
    [SerializeField, Range(0.05f, 2f)] private float duration = 0.5f;
    [SerializeField] private bool pulseOnAwake = false;
    private Coroutine pulseRoutine;
    private Vignette vignette;

    private void Start()
    {
        vignette = VFXTool.GetEffect<Vignette>();

        if (vignette == null)
        {
            Debug.LogError("[VignettePulsingEffect] Vignette not found in Volume Profile.");
            return;
        }

        if (pulseOnAwake)
            StartPulsing();

    }

    public void StartPulsing()
    {
        if (vignette == null)
            return;

        if (pulseRoutine == null)
            pulseRoutine = StartCoroutine(PulseLoop(minIntensity, maxIntensity, duration));
    }
    
    public void StartPulsing(float pMinIntensity, float pMaxIntensity, float pDuration)
    {
        if (vignette == null)
            return;

        if (pulseRoutine == null)
            pulseRoutine = StartCoroutine(PulseLoop( pMinIntensity, pMaxIntensity,pDuration));
    }

    public void SetPulsingParameters(float pMinIntensity, float pMaxIntensity, float pDuration)
    {
        minIntensity = pMinIntensity;
        maxIntensity = pMaxIntensity;
        duration = pDuration;
    }

    private IEnumerator PulseLoop(float pMinIntensity, float pMaxIntensity, float pDuration)
    {
        while (true)
        {
            yield return DOTween.To(
                    () => vignette.intensity.value,
                    x => vignette.intensity.value = x,
                    pMaxIntensity,
                    pDuration)
                .SetTarget(this)
                .WaitForCompletion();

            yield return DOTween.To(
                    () => vignette.intensity.value,
                    x => vignette.intensity.value = x,
                    pMinIntensity,
                    pDuration)
                .SetTarget(this)
                .WaitForCompletion();
        }
    }

    public void StopPulsing()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }

        DOTween.Kill(this);

        if (vignette != null)
        {
            vignette.intensity.value = minIntensity;
        }
    }
}