using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Unity.Collections;
public class GameSession : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    enum VolumeType
    {
        Master,
        Music,
        Sfx
    }
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private bool isMasterEnabled = true;
    private bool isMusicEnabled = true;
    private bool isSfxEnabled = true;
    public static GameSession Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        UpdateVolumes();
    }


    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
    }
    public void SetIsMasterEnabled(bool isEnabled)
    {
        isMasterEnabled = isEnabled;
    }
    public void SetIsMusicEnabled(bool isEnabled)
    {
        isMusicEnabled = isEnabled;
    }
    public void SetIsSfxEnabled(bool isEnabled)
    {
        isSfxEnabled = isEnabled;
    }
    private void UpdateVolumes()
    {
        if (isMasterEnabled)
        {
            if (masterVolume > 0)
            {
                audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
            }
            else
            {
                audioMixer.SetFloat("MasterVolume", -80);
            }
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", -80);
        }

        if (isMusicEnabled)
        {
            if (musicVolume > 0)
            {
                audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
            }
            else
            {
                audioMixer.SetFloat("MusicVolume", -80);
            }
        }
        else
        {
            audioMixer.SetFloat("MusicVolume", -80);
        }

        if (isSfxEnabled)
        {
            if (sfxVolume > 0)
            {
                audioMixer.SetFloat("SfxVolume", Mathf.Log10(sfxVolume) * 20);
            }
            else
            {
                audioMixer.SetFloat("SfxVolume", -80);
            }
        }
        else
        {
            audioMixer.SetFloat("SfxVolume", -80);
        }
    }
    public bool GetIsMasterEnabled()
    {
        return isMasterEnabled;
    }
    public bool GetIsMusicEnabled()
    {
        return isMusicEnabled;
    }
    public bool GetIsSfxEnabled()
    {
        return isSfxEnabled;
    }
    public float GetMasterVolume()
    {
        return masterVolume;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetSfxVolume()
    {
        return sfxVolume;
    }
}
