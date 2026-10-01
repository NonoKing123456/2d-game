using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private Collider2D springDetectCollider;
    [SerializeField] private float springForce = 5f;
    private PlayerControl playerControl;
    private Rigidbody2D playerRb;
    private Vector2 previousFootPosition;
    private bool hasPreviousFootPosition;
    AudioSource springAudioSource;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        springDetectCollider = GetComponent<Collider2D>();
        springAudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        PlayerControl currentPlayer = GameManager.instance != null ? GameManager.instance.CurrentPlayer : null;
        if (currentPlayer != playerControl)
        {
            playerControl = currentPlayer;
            playerRb = playerControl != null ? playerControl.GetComponent<Rigidbody2D>() : null;
            hasPreviousFootPosition = false;
        }

        if (playerControl == null || playerRb == null || !playerControl.isActiveAndEnabled)
        {
            hasPreviousFootPosition = false;
            return;
        }

        Vector2 currentFootPosition = playerControl.GroundCheckBottomPosition;
        if (hasPreviousFootPosition)
            TryBounce(ref currentFootPosition);

        previousFootPosition = currentFootPosition;
        hasPreviousFootPosition = true;
    }

    private void TryBounce(ref Vector2 currentFootPosition)
    {
        float springTop = springDetectCollider.bounds.max.y;
        float verticalTravel = previousFootPosition.y - currentFootPosition.y;
        if (verticalTravel <= 0f || previousFootPosition.y <= springTop || currentFootPosition.y > springTop || playerRb.linearVelocity.y > 0f)
            return;

        float crossingFraction = (previousFootPosition.y - springTop) / verticalTravel;
        float crossingX = Mathf.Lerp(previousFootPosition.x, currentFootPosition.x, crossingFraction);
        Bounds springBounds = springDetectCollider.bounds;
        float width = playerControl.GroundCheckWidth / 2f;
        if (crossingX < springBounds.min.x - width || crossingX > springBounds.max.x + width)
            return;

        playerRb.position += new Vector2(crossingX - currentFootPosition.x, springTop - currentFootPosition.y);
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
        playerRb.AddForce(Vector2.up * springForce, ForceMode2D.Impulse);
        springAudioSource.Play();
        animator.SetTrigger("Bounce");
        currentFootPosition = new Vector2(crossingX, springTop);
    }
}
