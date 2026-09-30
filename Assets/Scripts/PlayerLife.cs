using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;
    [SerializeField] private float invincibleTime = 1f;
    [SerializeField] private bool invincible = false;
    [SerializeField] private float invincibleUntil;
    [SerializeField] private float hurtDuration = 0.2f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float hurtUntil;
    [SerializeField] private bool hurting;
    [SerializeField] private bool dead;
    private int normalLayer;
    private int invincibleLayer;
    public bool Dead => dead;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool Hurting => hurting;
    public bool Invincible => invincible;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControl = GetComponent<PlayerControl>();
        normalLayer = gameObject.layer;
        invincibleLayer = LayerMask.NameToLayer("InvinciblePlayer");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (invincibleLayer >= 0 && enemyLayer >= 0)
        {
            Physics2D.IgnoreLayerCollision(invincibleLayer, enemyLayer, true);
        }
        else
        {
            Debug.LogError("PlayerLife requires the InvinciblePlayer and Enemy layers.", this);
        }

        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = maxHealth;
        invincible = false;
        hurting = false;
        dead = false;
    }

    void Update()
    {
        if (dead) return;
        CheckInvincibility();
        CheckHurt();
    }

    private void CheckInvincibility()
    {
        if (invincible && Time.time >= invincibleUntil)
        {
            invincible = false;
            gameObject.layer = normalLayer;
        }
    }

    private void CheckHurt()
    {
        if (hurting && Time.time >= hurtUntil)
        {
            hurting = false;
            playerControl.SetCanMove(true);
        }
    }
    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision.transform);
        }
    }

    private void TakeDamage(Transform enemyTransform)
    {
        if (dead || invincible)
        {
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - 1);
        if (currentHealth == 0)
        {
            Die();
            return;
        }

        invincible = true;
        invincibleUntil = Time.time + invincibleTime;
        if (invincibleLayer >= 0) gameObject.layer = invincibleLayer;
        EnterHurtState(enemyTransform);
    }

    private void EnterHurtState(Transform enemyTransform)
    {
        hurting = true;
        hurtUntil = Time.time + hurtDuration;
        playerControl.CancelSlashing();
        playerControl.SetCanMove(false);
        rb.linearVelocity = Vector2.zero; // Reset velocity before applying knockback
        rb.AddForce(new Vector2(-Mathf.Sign(enemyTransform.position.x - transform.position.x) * knockbackForce, 0), ForceMode2D.Impulse);
    }

    private void Die()
    {
        if (dead) return;
        dead = true;
        hurting = false;
        invincible = false;
        hurtUntil = 0f;
        invincibleUntil = 0f;
        playerControl.CancelSlashing();
        playerControl.SetCanMove(false);
        playerControl.enabled = false;
        rb.linearVelocity = Vector2.zero;
        if (invincibleLayer >= 0) gameObject.layer = invincibleLayer;
        GameManager.Instance.GameOver();
    }

}
