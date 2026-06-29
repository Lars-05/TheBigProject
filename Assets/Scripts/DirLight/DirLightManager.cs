using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class DirLightManager : MonoBehaviour
{
    private static DirLightManager _instance;
    private Light _light;

    private Coroutine _intensityRoutine;

    private void Awake()
    {
        _instance = this;
        _light = GetComponent<Light>();
    }
    
    public static void SetIntensity(float targetIntensity, float duration)
    {
        if (_instance == null)
            return;

        if (_instance._intensityRoutine != null)
            _instance.StopCoroutine(_instance._intensityRoutine);

        _instance._intensityRoutine = _instance.StartCoroutine(
            _instance.LerpIntensity(targetIntensity, duration));
    }

    private IEnumerator LerpIntensity(float targetIntensity, float duration)
    {
        float startIntensity = _light.intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _light.intensity = Mathf.Lerp(
                startIntensity,
                targetIntensity,
                elapsed / duration);

            yield return null;
        }

        _light.intensity = targetIntensity;
        _intensityRoutine = null;
    }
}