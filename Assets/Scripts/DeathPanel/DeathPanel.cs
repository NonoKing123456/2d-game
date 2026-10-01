using UnityEngine;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private AudioSource deathAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        deathAudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        deathAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
