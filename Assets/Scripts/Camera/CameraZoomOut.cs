using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Animator))]
public class CameraZoomOut : MonoBehaviour
{
    private static CameraZoomOut _instance;

    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private AnimationClip zoomOutClip;

    private Camera _camera;
    private Animator _animator;

    private void Awake()
    {
        _instance = this;

        _camera = GetComponent<Camera>();
        _animator = GetComponent<Animator>();

        _camera.enabled = false;
    }

    public static void Play()
    {
        if (_instance != null)
            _instance.StartCoroutine(_instance.ZoomOutCoroutine());
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
        SceneController.GotoScene("MainMenu");
    }
}