using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Dead,
        Hurt
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyPatrol enemyPatrol;
    [SerializeField] private EnemyChase enemyChase;
    [SerializeField] private Collider2D enemyCollider;

    [Header("Attack")]
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int attackDamage = 1;

    [Header("State")]
    [SerializeField] private EnemyState currentState = EnemyState.Patrol;
    [SerializeField] private float hurtDuration = 0.2f;

    private int currentHealth;

    private float attackTimer;
    private float hurtUntil;
    private Rigidbody2D rb;
    private static PhysicsMaterial2D noFrictionMaterial;


    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyChase = GetComponent<EnemyChase>();
        rb = GetComponent<Rigidbody2D>();

        if (enemyCollider.sharedMaterial == null)
        {
            if (noFrictionMaterial == null)
            {
                noFrictionMaterial = new PhysicsMaterial2D("Enemy body without friction")
                {
                    friction = 0f,
                    bounciness = 0f
                };
            }

            enemyCollider.sharedMaterial = noFrictionMaterial;
        }

        ApplyMovementState();
    }

    void Update()
    {
        if (currentState == EnemyState.Dead || currentState == EnemyState.Attack)
        {
            return;
        }

        if (currentState == EnemyState.Hurt)
        {
            if (Time.time >= hurtUntil)
            {
                SetState(EnemyState.Chase);
            }

            return;
        }

        if (currentState == EnemyState.Patrol &&
            Vector2.Distance(transform.position, player.position) < detectRange)
        {
            SetState(EnemyState.Chase);
        }
    }

    public void EnterHurt(float knockbackForce)
    {
        if (currentState == EnemyState.Dead) return;

        hurtUntil = Time.time + hurtDuration;
        SetState(EnemyState.Hurt);
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * knockbackForce, 0f), ForceMode2D.Impulse);
    }

    private void SetState(EnemyState nextState)
    {
        if (currentState == nextState) return;

        currentState = nextState;
        ApplyMovementState();
    }

    private void ApplyMovementState()
    {
        enemyPatrol.enabled = currentState == EnemyState.Patrol;
        enemyChase.enabled = currentState == EnemyState.Chase;
    }
}
