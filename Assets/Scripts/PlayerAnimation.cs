using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    private PlayerControl playerControl;
    private PlayerLife playerLife;
    private SpriteRenderer spriteRenderer;
    private bool deathTriggered;
    [SerializeField] private Color InvincibleColor;
    [SerializeField] private Color HurtColor;
    [SerializeField] private float InvincibleColorLerpSpeed = 5f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerControl = GetComponent<PlayerControl>();
        playerLife = GetComponent<PlayerLife>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerLife.Dead)
        {
            if (!deathTriggered)
            {
                deathTriggered = true;
                animator.SetBool("Hurting", false);
                animator.ResetTrigger("Slash");
                animator.SetTrigger("Die");
                spriteRenderer.color = Color.white;
            }
            return;
        }

        animator.SetFloat("Speed", Math.Abs(rb.linearVelocity.x));
        animator.SetBool("Grounded", playerControl.GetGrounded());
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        // 旋转角色
        float inputX = playerControl.HorizontalInput;
        if (Math.Abs(inputX) > 0.1f && Math.Sign(rb.linearVelocity.x) == Math.Sign(inputX) && Math.Abs(rb.linearVelocity.x) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Math.Sign(inputX) * Math.Abs(scale.x);
            transform.localScale = scale;
        }
        HurtAndInvincible();
    }

    private void HurtAndInvincible()
    {
        animator.SetBool("Hurting", playerLife.Hurting);
        if (playerLife.Hurting)
        {
            spriteRenderer.color = HurtColor;
        }
        else if (playerLife.Invincible)
        {
            float t = Mathf.PingPong(
                Time.time * InvincibleColorLerpSpeed, 1f);

            spriteRenderer.color = Color.Lerp(InvincibleColor, Color.white, t);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

    }

}
