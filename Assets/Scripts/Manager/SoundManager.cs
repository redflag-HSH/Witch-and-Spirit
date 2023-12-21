
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixerGroup MasterMixerGroup, SFXMixerGroup, BGMMixerGroup;
    public AudioSource BGMSource;

    private readonly Queue<AudioSource> _unusedSources = new();
    private readonly List<AudioSource> _usingSources = new();

    private void SetVolume(string name, float volume)
    {
        if (volume <= 0) volume = 0.000001f;
        MasterMixerGroup.audioMixer.SetFloat(name, Mathf.Log10(volume) * 20f);
    }

    private float GetVolume(string name)
    {
        if (MasterMixerGroup.audioMixer.GetFloat(name, out var value)) 
            return Mathf.Pow(10, value / 20);
        return 0f;
    }

    public void SetMasterVolume(float volume) => SetVolume("Master", volume);
    public void SetSFXVolume(float volume) => SetVolume("SFX", volume);
    public void SetBGMVolume(float volume) => SetVolume("BGM", volume);
    public float GetMasterVolume() => GetVolume("Master");
    public float GetSFXVolume() => GetVolume("SFX");
    public float GetBGMVolume() => GetVolume("BGM");

    private AudioSource GetSource()
    {
        if (_unusedSources.Count > 0)
        {
            return _unusedSources.Dequeue();
        }
        else
        {
            var source = new GameObject("Audio Source").AddComponent<AudioSource>();
            return source;
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public void PlaySFX(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f, Transform parent = null)
    {
        var source = GetSource();
        source.transform.SetParent(parent);
        source.transform.position = pos;

        source.outputAudioMixerGroup = SFXMixerGroup;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        _usingSources.Add(source);
    }

    public void PlaySFX(AudioClip clip, Vector3 pos, float volume = 1f, Transform parent = null)
    {
        PlaySFX(clip, pos, volume, 1f, parent);
    }

    public void PlaySFX(AudioClip clip, Transform parent, float volume = 1f, float pitch = 1f)
    {
        PlaySFX(clip, parent.position, volume, pitch, parent);
    }

    private void Update()
    {
        HashSet<AudioSource> movingSources = new();
        foreach (var source in _usingSources)
        {
            if (!source.isPlaying)
            {
                movingSources.Add(source);
            }
        }
        _usingSources.RemoveAll(s => movingSources.Contains(s));
        foreach (var source in movingSources)
        {
            _unusedSources.Enqueue(source);
        }
    }
}