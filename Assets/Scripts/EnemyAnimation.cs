using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{   
    private Animator animator;
    private EnemyBrain enemyBrain;
    private EnemyHealth enemyHealth;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hurtColor;
    private int basicStatusInt;
    
    void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (enemyBrain.CurrentState == EnemyBrain.EnemyState.Hurt || enemyBrain.CurrentState == EnemyBrain.EnemyState.Dead)
        {
            spriteRenderer.color = hurtColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        BasicStatusChange();
    }

    private void BasicStatusChange()
    {
        basicStatusInt = enemyBrain.CurrentState switch
        {
            EnemyBrain.EnemyState.Patrol => 1,
            EnemyBrain.EnemyState.Chase => 2,
            EnemyBrain.EnemyState.Hurt => 3,
            EnemyBrain.EnemyState.Dead => 4,
            _ => 0
        };
        animator.SetInteger("BasicStatus", basicStatusInt);
        if (basicStatusInt == 0)
        {
            animator.SetInteger("BasicStatus", 1);
        }
    }
}
