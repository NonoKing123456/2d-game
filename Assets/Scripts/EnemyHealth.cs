using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private float invincibleTime = 1f;
    [SerializeField] private bool invincible = false;
    [SerializeField] private float hurtDuration = 0.2f;
    [SerializeField] private float knockbackForce = 5f;

    private EnemyBrain brain;
    private Rigidbody2D rb;
    private Collider2D bodyCollider;
    private float invincibleUntil;
    private float hurtUntil;
    public bool hurting;
    private static PhysicsMaterial2D noFrictionMaterial;

    private void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        currentHealth = maxHealth;

        if (bodyCollider.sharedMaterial == null)
        {
            if (noFrictionMaterial == null)
            {
                noFrictionMaterial = new PhysicsMaterial2D("Enemy body without friction")
                {
                    friction = 0f,
                    bounciness = 0f
                };
            }

            bodyCollider.sharedMaterial = noFrictionMaterial;
        }
    }

    private void Update()
    {
        if (invincible && Time.time >= invincibleUntil)
        {
            invincible = false;
        }

        if (hurting && Time.time >= hurtUntil)
        {
            hurting = false;
            brain.RecoverFromHurt();
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincible || currentHealth <= 0)
        {
            return;
        }
        currentHealth -= damage;
        Debug.Log("Enemy Health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        invincible = true;
        invincibleUntil = Time.time + invincibleTime;
        hurting = true;
        hurtUntil = Time.time + hurtDuration;

        brain.EnterHurt();
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * knockbackForce, 0f), ForceMode2D.Impulse);
    }

    private void Die()
    {
        hurting = false;
        brain.EnterDead();
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        Debug.Log("Enemy Died!");
    }

    public void ClearCorpse()
    {
        Destroy(gameObject);
    }
}
