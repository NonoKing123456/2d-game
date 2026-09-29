using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private float invincibleTime = 1f;
    [SerializeField] private bool invincible = false;
    [SerializeField] private float knockbackForce = 5f;
    private EnemyBrain brain;
    private void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        currentHealth = maxHealth;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        if (invincible)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }

        brain.EnterHurt(knockbackForce);
        
        if (!invincible)
        {
            invincible = true;
            Invoke(nameof(ResetInvincibility), invincibleTime);
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died!");
    }

    private void ResetInvincibility()
    {
        invincible = false;
    }
}
