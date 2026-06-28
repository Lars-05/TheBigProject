using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    private static AudioSource _musicSource;
    private static AudioSource _sfx;
    private static readonly Dictionary<string, AudioClip> _audios = new();

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
        
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        
        _musicSource = _instance.gameObject.AddComponent<AudioSource>();
        _sfx = _instance.gameObject.AddComponent<AudioSource>();
        
        foreach (var sound in Resources.LoadAll<AudioClip>("Audio"))
            _audios[sound.name] = sound;
    }

    public static void PlaySound(string soundName)
    {
        _sfx.PlayOneShot(_audios[soundName]);
    }

    public static void PlaySound(string soundName, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(_audios[soundName], position);
    }

    public static void PlaySound(string soundName, Vector3 position, bool ignoreListenerPause)
    {
        _sfx.ignoreListenerPause = ignoreListenerPause;
        _sfx.PlayOneShot(_audios[soundName]);
    }

    public static void PlaySound(string soundName, GameObject gameObject)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = _audios[soundName];
        source.Play();
        Destroy(source, source.clip.length);
    }
    
    /// <summary>
    /// Helper Tools Added by Me (Lars)
    /// </summary>
    /// ----------
    public static void PlaySoundExternal(AudioClip audioClip, AudioSource source)
    {
        source.clip = audioClip;
        source.Play();
        Destroy(source, source.clip.length);
    }
    public static AudioSource InterceptSource(GameObject gameObject)
    {
        return gameObject.AddComponent<AudioSource>();
    }
    public static AudioClip GetAudioClip(string soundName)
    {
        return _audios[soundName];
    }
    /// ----------

    public void PlayMusic(string soundName)
    {
        _musicSource.PlayOneShot(_audios[soundName]);
    }
}
