using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Dead,
        Hurt
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemyPatrol enemyPatrol;
    [SerializeField] private EnemyChase enemyChase;

    [Header("Attack")]
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private bool onlyDetectFront = true;

    [Header("State")]
    [SerializeField] private EnemyState currentState = EnemyState.Patrol;

    public EnemyState CurrentState => currentState;


    void Awake()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyChase = GetComponent<EnemyChase>();
        ApplyMovementState();
    }

    public void SetPlayer(Transform playerTarget)
    {
        player = playerTarget;
        enemyChase.SetPlayer(playerTarget);
    }

    void Update()
    {
        if (player == null || currentState != EnemyState.Patrol) return;

        bool playerInRange = Vector2.Distance(transform.position, player.position) < detectRange;
        bool playerInFront = (player.position.x - transform.position.x) * transform.lossyScale.x > 0f;

        if (playerInRange && (!onlyDetectFront || playerInFront))
            SetState(EnemyState.Chase);
    }

    public void EnterHurt()
    {
        if (currentState == EnemyState.Dead) return;
        SetState(EnemyState.Hurt);
    }

    public void RecoverFromHurt()
    {
        if (currentState == EnemyState.Hurt)
        {
            SetState(EnemyState.Chase);
        }
    }

    public void EnterDead()
    {
        SetState(EnemyState.Dead);
    }

    private void SetState(EnemyState nextState)
    {
        if (currentState == nextState) return;

        currentState = nextState;
        ApplyMovementState();
        if (currentState == EnemyState.Dead)
        {
            enemyPatrol.enabled = false;
            enemyChase.enabled = false;
        }
    }

    private void ApplyMovementState()
    {
        enemyPatrol.enabled = currentState == EnemyState.Patrol;
        enemyChase.enabled = currentState == EnemyState.Chase;
    }
}
