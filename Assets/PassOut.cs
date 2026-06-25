using System.Collections;
using TMPro;
using UnityEngine;
public class PassOut : MonoBehaviour
{
    
    [Header("Vignette Pulse")]
    [SerializeField, Range(0, 1)] private float minIntensity;
    [SerializeField, Range(0, 1)] private float maxIntensity;
    [SerializeField] private float duration;
    
    [Header("Signal Lost Text")]
    [SerializeField] private TextMeshProUGUI signalLostText;

    [Header("Player")] 
    [SerializeField] private GameObject player;
    
    [Header("Animation")] 
    [SerializeField] private AnimationClip passOutAnimation;
    private MeshRenderer  _meshRenderer;
    private Animator _animator;
    
    [Header("Fade Out")]
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private CanvasGroup canvasGroup;

    private AudioListener listener;
    
  
    private void OnEnable()
    {
        EventBus.OnPlayerPassedOut += StartCoroutine;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerPassedOut -= StartCoroutine;
    }

    private void Start()
    {
        _animator = player.GetComponentInChildren<Animator>();
        listener = Camera.main.GetComponent<AudioListener>();
        canvasGroup.alpha = 0;
        signalLostText.gameObject.SetActive(false);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        StartCoroutine();
    }

    private void StartCoroutine()
    {
        StartCoroutine(PassOutCoroutine());
    }


    public IEnumerator PassOutCoroutine()
    {
        SanityManager.isDead = true;
        canvasGroup.gameObject.SetActive(true);
        AudioManager.PlaySound("DeathStatic");
        signalLostText.gameObject.SetActive(true);
        
        _animator.Play(passOutAnimation.name);
 
        
        VFXManager.interlacingShaderEffect.SetIntensity(1, passOutAnimation.length);
        VFXManager.vignetteController.StartPulsing(
            minIntensity,
            maxIntensity,
            duration);
        
  
        yield return new WaitForSeconds(passOutAnimation.length);
      
        AudioManager.PlaySound("Thud");
        yield return new WaitForSeconds(3);
        signalLostText.gameObject.SetActive(false);
        AudioSource deathAudioSource = AudioManager.InterceptSource(gameObject);
        deathAudioSource.clip = AudioManager.GetAudioClip("DeathScreen");
        deathAudioSource.Play();
        deathAudioSource.loop = true;
        deathAudioSource.volume = 0;
        float elapsed = 0f;
        
        canvasGroup.gameObject.SetActive(true);
        
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeOutDuration);
            deathAudioSource.volume = Mathf.Clamp01(elapsed / fadeOutDuration);
            yield return null;
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}