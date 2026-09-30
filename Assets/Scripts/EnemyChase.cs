using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damage = 1;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPlayer(Transform playerTarget)
    {
        player = playerTarget;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        float horizontalSpeed = Mathf.Sign(player.position.x - transform.position.x) * chaseSpeed;
        rb.linearVelocity = new Vector2(horizontalSpeed, rb.linearVelocity.y);

        if (horizontalSpeed > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalSpeed < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
    }
}
