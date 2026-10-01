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
        if (gameState != GameManager.Instance.CurrentState)
        {
            ChangeState();
        }
    }
    

    public void PlayDeathSound()
    {
        if (gameState == GameManager.GameState.GameOver)
        {
            sfxSource.PlayOneShot(failClip);
        }
    }
    private void ChangeState()
    {
        gameState = GameManager.Instance.CurrentState;
        if (gameState == GameManager.GameState.Playing)
        {
            backgroundMusicSource.Play();
            ambientSoundSource.Play();
        }
        else if (gameState == GameManager.GameState.GameOver)
        {
            backgroundMusicSource.Stop();
            PlayDeathSound();
        }
    }
}
