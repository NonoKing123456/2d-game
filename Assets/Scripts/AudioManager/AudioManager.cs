using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource ambientSoundSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip failClip;
    public GameManager.GameState gameState;
    private void Awake()
    {
    }
    private void Update()
    {
        if (gameState != GameManager.instance.CurrentState){
            UpdateState();
        }
    }
    

    public void PlayDeathSound()
    {
        if (gameState == GameManager.GameState.GameOver)
        {
            sfxSource.PlayOneShot(failClip);
        }
    }
    private void UpdateState()
    {
        if (gameState == GameManager.GameState.Paused)
        {
            AudioListener.pause = false;
        }
        gameState = GameManager.instance.CurrentState;
        if (gameState == GameManager.GameState.Playing)
        {
            if (!backgroundMusicSource.isPlaying){
                backgroundMusicSource.Play();
            }
        }
        else if (gameState == GameManager.GameState.GameOver)
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
}
