using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwapFX : MonoBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private Animator _animator;

    [SerializeField] private AnimationClip _openClip;
    [SerializeField] private AnimationClip _closeClip;

    private static SceneSwapFX _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded+= OnSceneLoaded;

        _panel.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(OpenCoroutine());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(OpenCoroutine());
    }

    public static IEnumerator Close()
    {
        return _instance.CloseCoroutine();
    }
    
    public static IEnumerator Open()
    {
        return _instance.OpenCoroutine();
    }

    private IEnumerator OpenCoroutine()
    {
        _panel.gameObject.SetActive(true);

        PlayClip(_openClip);

        yield return new WaitForSecondsRealtime(_openClip.length);

        _panel.gameObject.SetActive(false);
    }

    private IEnumerator CloseCoroutine()
    {
        _panel.gameObject.SetActive(true);

        PlayClip(_closeClip);

        yield return new WaitForSecondsRealtime(_closeClip.length);
    }

    private void PlayClip(AnimationClip clip)
    {
        _animator.Play(clip.name, 0, 0f);
        _animator.Update(0f);
    }
}