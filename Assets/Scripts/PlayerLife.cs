using Unity.VisualScripting;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHealth--;
            Debug.Log("Player Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    private void Die()
    {
        Debug.Log("Player Died");
        // Add death logic here (e.g., respawn, game over screen, etc.)
    }
}
