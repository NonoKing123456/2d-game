using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource moveSource;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip slash;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip die;
    public void PlayJump()
    {
        source.PlayOneShot(jump);
    }
    public void PlaySlash()
    {
        source.PlayOneShot(slash);
    }
    public void PlayHurt()
    {
        source.PlayOneShot(hurt);
    }
    public void PlayDie()
    {
        source.PlayOneShot(die);
    }
    public void SetMovePlaying(bool moving)
    {
        if (moving && !moveSource.isPlaying)
            moveSource.Play();
        else if (!moving && moveSource.isPlaying)
            moveSource.Stop();
    }
}
