using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Animator))]
public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private AnimationClip zoomOutClip;

    private Camera _camera;
    private Animator _animator;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _animator = GetComponent<Animator>();

        _camera.enabled = false;
    }

    private void OnEnable() => EventBus.OnWin += PlayZoomOut;
    private void OnDisable() => EventBus.OnWin -= PlayZoomOut;

    public void PlayZoomOut()
    {
        StartCoroutine(ZoomOutCoroutine());
    }

    private IEnumerator ZoomOutCoroutine()
    {
        if (gameplayCamera != null)
            gameplayCamera.enabled = false;

        _camera.enabled = true;

        _animator.Play(zoomOutClip.name);

        DirLightManager.SetIntensity(1f, zoomOutClip.length);

        yield return new WaitForSeconds(zoomOutClip.length);

        _camera.enabled = false;

        if (gameplayCamera != null)
            gameplayCamera.enabled = true;
    }
}