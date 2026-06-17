using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System.Collections;

public class VignettePulsingEffect : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float minIntensity;

    [SerializeField, Range(0f, 1f)]
    private float maxIntensity;

    [SerializeField, Range(0f, 2f)]
    private float duration;

    private Coroutine pulseRoutine;
    private Vignette vignette;

    private void Start()
    {
        vignette = VFXTool.GetEffect(VFXTool.VFXType.Vignette) as Vignette;

        if (vignette != null)
            StartPulsing();
    }

    private void StartPulsing()
    {
        if (pulseRoutine == null)
            pulseRoutine = StartCoroutine(PulseLoop());
    }

    private IEnumerator PulseLoop()
    {
        while (true)
        {
            DOTween.To(
                () => vignette.intensity.value,
                x => vignette.intensity.value = x,
                maxIntensity,
                duration);

            yield return new WaitForSeconds(duration);

            DOTween.To(
                () => vignette.intensity.value,
                x => vignette.intensity.value = x,
                minIntensity,
                duration);

            yield return new WaitForSeconds(duration);
        }
    }

    private void StopPulsing()
    {
        if (pulseRoutine != null)
        {
            StopCoroutine(pulseRoutine);
            pulseRoutine = null;
        }

        DOTween.Kill(vignette);
    }
}