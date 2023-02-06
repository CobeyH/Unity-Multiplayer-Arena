using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] soundEffects;
    [SerializeField]
    private Sound[] musicTracks;


    public static AudioManager instance;

    public AudioMixerGroup sFXMixer;
    public AudioMixerGroup musicMixer;

    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    // Start is called before the first frame update
    void Awake()
    {
        // Audio manager implements the singleton pattern so that music doesn't stop between scenes.
        if (instance != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        AddAudioSources(soundEffects, sFXMixer);
        AddAudioSources(musicTracks, musicMixer);
        // Restore the volume from previous play session
        SetMusicVolume(PlayerPrefs.GetFloat("Volume", 1));
    }

    void Start()
    {
        PlayMusic();
    }

    void PlayMusic()
    {
        Sound currentSong = musicTracks[UnityEngine.Random.Range(0, musicTracks.Length - 1)];
        currentSong.source.Play();
        Invoke(nameof(PlayMusic), currentSong.clip.length);
    }

    void AddAudioSources(Sound[] clips, AudioMixerGroup mixer)
    {
        foreach (Sound s in clips)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixer;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
            return;
        }
        else
        {
            Debug.LogWarning("Sound not found with name: " + name);
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
            return;
        }
    }

    public void PauseAllSounds()
    {
        activeAudioSources.Clear();
        foreach (Sound s in soundEffects)
        {
            if (s.source.isPlaying)
            {
                activeAudioSources.Add(s.source);
                s.source.Pause();
            }
        }
    }

    public void ResumeSounds()
    {
        foreach (AudioSource s in activeAudioSources)
        {
            s.UnPause();
        }
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("Volume", newVolume);
        // Scale the volume so that it isn't on a logarithmic scale
        newVolume = Mathf.Log(newVolume) * 20;
        musicMixer.audioMixer.SetFloat("Volume", newVolume);
    }

}