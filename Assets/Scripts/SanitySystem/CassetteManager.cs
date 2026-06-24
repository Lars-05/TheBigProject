using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class CassetteManager : MonoBehaviour
{
    static bool isPlaying = false;
    [SerializeField] private GameObject _player;
    [SerializeField] private int _sanityPerSecond;
    private AudioSource _cassetteAudioSource;
    private AudioSource _songAudioSource;
    private AudioClip _cassetteStartAudioClip;
    private AudioClip _cassetteStopAudioClip;
    private AudioClip _cassetteSongAudioClip;
    private AudioClip _cassetteStaticAudioClip;
    public void Start()
    {
        ConfigureAudioSources();
    }
   public void OnCassettePlayerButtonClicked()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(StartUpCassette());
            return;
        }
        isPlaying = false;
        StopPlaying();
    }
   
    private IEnumerator StartUpCassette()
    {
        
        _cassetteAudioSource.clip = _cassetteStartAudioClip;
        _cassetteAudioSource.Play();

        yield return new WaitForSeconds(_cassetteStartAudioClip.length);

        _songAudioSource.clip = _cassetteSongAudioClip;

        InvokeRepeating(nameof(IncreaseSanity), 1f, 1f);
    }
    void StopPlaying()
    {
        CancelInvoke(nameof(IncreaseSanity));
        _songAudioSource.Pause();
        _cassetteAudioSource.clip = _cassetteStopAudioClip;
        _cassetteAudioSource.Play();
    }
    
    private void ConfigureAudioSources()
    {
        _cassetteStartAudioClip = AudioManager.GetAudioClip("CassetteInsert");
        _cassetteStopAudioClip = AudioManager.GetAudioClip("CassetteTakeOut");
        _cassetteSongAudioClip = AudioManager.GetAudioClip("CaliforniaGurls");
        _cassetteStaticAudioClip = AudioManager.GetAudioClip("Static");

        
        _cassetteAudioSource = AudioManager.InterceptSource(_player);
        _cassetteAudioSource.playOnAwake = false;
        _cassetteAudioSource.loop = false;
        
        _songAudioSource = AudioManager.InterceptSource(_player);
        _songAudioSource.playOnAwake = false;
        _songAudioSource.loop = true;
        _songAudioSource.clip = _cassetteSongAudioClip;
    }

    private void IncreaseSanity()
    {
        if (SanityManager.isDead)
        {
            _songAudioSource.Stop();
            return;
        }

        
        if (BurningManAI.isHunting)
        {
            if (_songAudioSource.clip != _cassetteStaticAudioClip)
            {
                _songAudioSource.Stop();
                _songAudioSource.clip = _cassetteStaticAudioClip;
                _songAudioSource.Play();
            }
            return;
        }

        if (_songAudioSource.clip != _cassetteSongAudioClip)
        {
            _songAudioSource.Stop();
            _songAudioSource.clip = _cassetteSongAudioClip;
            _songAudioSource.Play();
        }

        if (!_songAudioSource.isPlaying)
            _songAudioSource.Play();

        SanityManager.GainSanity?.Invoke(_sanityPerSecond);
    }
    
    private void Play(InputAction.CallbackContext _)
    {
        if (SanityManager.isDead)
            return;
        OnCassettePlayerButtonClicked();
    }

    private IDisposable disposable; // null
    private void OnEnable()
    {
        disposable = InputManager.Instance.BindPerformed("PlayCassette",Play);
    }
    private void OnDisable()
    {
        disposable.Dispose();
    }

}
