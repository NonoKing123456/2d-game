using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damage = 1;
    [SerializeField] private float stoppingGap = 0.1f;
    private Collider2D playerBody;
    private Collider2D enemyBody;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        playerBody = player.GetComponent<Collider2D>();
        enemyBody = GetComponent<Collider2D>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = player.position.x - transform.position.x;
        float bodySpacing = playerBody.bounds.extents.x + enemyBody.bounds.extents.x + stoppingGap;
        float horizontalSpeed = Mathf.Abs(deltaX) > bodySpacing
            ? Mathf.Sign(deltaX) * chaseSpeed
            : 0f;
        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);

        if (deltaX > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (deltaX < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
}
