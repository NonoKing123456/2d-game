using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource ambientSoundSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip failClip;
    public static AudioManager Instance { get; private set; }
    public GameManager.GameState gameState;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        UpdateState(GameManager.GameState.Playing);
    }
    private void Update()
    {
        if (gameState != GameManager.instance.CurrentState){
            UpdateState(GameManager.instance.CurrentState);
        }
    }
    

    public void PlayDeathSound()
    {
        if (gameState == GameManager.GameState.GameOver)
        {
            sfxSource.PlayOneShot(failClip);
        }
    }
    private void UpdateState(GameManager.GameState newState)
    {
       
        if (gameState == GameManager.GameState.Paused)
        {
            AudioListener.pause = false;
            backgroundMusicSource.ignoreListenerPause = false;
            ambientSoundSource.ignoreListenerPause = false;
        }
        gameState = newState;
        if (gameState == GameManager.GameState.Playing)
        {
            if (!backgroundMusicSource.isPlaying){
                backgroundMusicSource.Play();
            }
        }
        if (gameState == GameManager.GameState.GameOver)
        {
            backgroundMusicSource.Stop();
            PlayDeathSound();
        }
        else if (gameState == GameManager.GameState.Paused)
        {
            AudioListener.pause = true;
            ambientSoundSource.ignoreListenerPause = true;
        }
    }
    public void Unpause()
    {
        AudioListener.pause = false;
        backgroundMusicSource.ignoreListenerPause = false;
        ambientSoundSource.ignoreListenerPause = false;
    }

    internal void ReplayMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
            backgroundMusicSource.Play();
        }
    }
}
