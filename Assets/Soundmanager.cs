using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmanager : MonoBehaviour
{
    private void OnDestroy()
    {
        _instance = null;
    }

    //싱글톤만들기.
    private static Soundmanager _instance = null;
    public static Soundmanager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Soundmanager)) as Soundmanager;

                if (_instance == null)
                {
                    //Debug.Log("Player script Error");
                }
            }
            return _instance;
        }
    }
    [SerializeField]
    AudioSource audios;
    [SerializeField]
    AudioSource audios2;
    private Dictionary<string, AudioClip> Sounds = new Dictionary<string, AudioClip>();
    public void PlayerSound(string _key,float soundvalue =0.6f)
    {
        if (Sounds.TryGetValue(_key, out var sound))
        {
            audios.PlayOneShot(sound,soundvalue);
            return;
        }
        AudioClip value = Resources.Load<AudioClip>(_key);
        Sounds.Add(_key, value);
        audios.PlayOneShot(Sounds[_key]);
    }
    public void PlayerSound2(string _key,float soundvalue =0.6f)
    {
        if (Sounds.TryGetValue(_key, out var sound))
        {
            audios2.PlayOneShot(sound,soundvalue);
            return;
        }
        AudioClip value = Resources.Load<AudioClip>(_key);
        Sounds.Add(_key, value);
        audios2.PlayOneShot(Sounds[_key]);
    }
}
