using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private GameObject attackHitbox;

    private float leftPatrolX;
    private float rightPatrolX;
    private bool movingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        leftPoint = transform.Find("LeftPatrolPoint");
        rightPoint = transform.Find("RightPatrolPoint");
        leftPatrolX = Mathf.Min(leftPoint.position.x, rightPoint.position.x);
        rightPatrolX = Mathf.Max(leftPoint.position.x, rightPoint.position.x);
        attackHitbox = transform.Find("AttackHitbox").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return;

        if (movingRight)
        {
            rb.linearVelocity = new Vector2(patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x >= rightPatrolX)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x <= leftPatrolX)
            {
                movingRight = true;
                Flip();
            }
        }
        
    }
        private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        //attackHitbox.transform.localPosition = new Vector3(-attackHitbox.transform.localPosition.x, attackHitbox.transform.localPosition.y, attackHitbox.transform.localPosition.z);
    }
}
