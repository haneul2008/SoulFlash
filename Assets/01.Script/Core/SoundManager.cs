using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum AudioType
{
    SFX, BGM
}

[Serializable]
public struct Sound
{
    public AudioClip clip;
    public float volumeMultiplier;
    public AudioType type;
    public float pitch;
    public bool loop;
}

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private List<AudioSource> _audioSource = new List<AudioSource>();
    [SerializeField] private Sound _uiClockSound;

    private List<AudioType> _audioTypes = new List<AudioType>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for(int i = 0; i <  _audioSource.Count; i++)
            _audioTypes.Add(AudioType.SFX);
    }
    public void AddAudioAndPlay(Sound sound)
    {
        if(sound.type == AudioType.BGM)
        {
            for (int i = 0; i < _audioSource.Count; i++)
            {
                if (_audioTypes[i] != AudioType.BGM) continue;

                PlayClip(sound, _audioSource[i]);
                _audioTypes[i] = sound.type;

                return;
            }
        }
        else
        {
            for (int i = 0; i < _audioSource.Count; i++)
            {
                if (_audioSource[i].isPlaying) continue;

                PlayClip(sound, _audioSource[i]);
                _audioTypes[i] = sound.type;

                return;
            }
        }
       
        AudioSource addAudioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.Add(addAudioSource);
        _audioTypes.Add(sound.type);
        PlayClip(sound, addAudioSource);

        return;
    }

    private void PlayClip(Sound sound, AudioSource audioSource)
    {
        audioSource.volume = GameManager.instance.SoundVolume * sound.volumeMultiplier;
        audioSource.pitch = sound.pitch;

        if (sound.type == AudioType.BGM)
        {
            CheckTypeAndPlay(sound, audioSource, true);
        }
        else
        {
            CheckTypeAndPlay(sound, audioSource, sound.loop);
        }
    }
    private void CheckTypeAndPlay(Sound sound, AudioSource audioSource, bool loop)
    {
        audioSource.clip = sound.clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopSound(Sound sound)
    {
        foreach(AudioSource audioSource in _audioSource)
        {
            if (sound.clip != audioSource.clip) continue;

            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    public void UiClickSound()
    {
        AddAudioAndPlay(_uiClockSound);
    }
}