using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

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
        _cassetteAudioSource.Stop();
        _songAudioSource.Play();
        InvokeRepeating(nameof(IncreaseSanity), 1,1 );
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
        SanityDisplayer.GainSanity?.Invoke(_sanityPerSecond);
    }

}
