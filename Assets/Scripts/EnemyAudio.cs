using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioClip slashed;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayDie()
    {
        source.PlayOneShot(die);
    }

    public void PlayHurt()
    {
        source.PlayOneShot(hurt);
    }
    
    public void PlaySlashed()
    {
        source.PlayOneShot(slashed);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
