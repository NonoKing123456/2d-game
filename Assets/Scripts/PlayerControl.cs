using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * 玩家控制器类，继承自MonoBehaviour，用于处理玩家的移动、跳跃和攻击等行为
 */
public class PlayerControl : MonoBehaviour
{
    [Header("Player Movement")]
    public float speed = 10f;
    public float jumpForce = 10f;
    [SerializeField] private float groundCheckwidth = 0.1f;
    [SerializeField] private float groundCheckHeight = 0.1f;

    private LayerMask groundLayer;
    private PlayerAudio playerAudio;


    private Rigidbody2D rb;
    private static PhysicsMaterial2D noFrictionMaterial;
    private Animator animator;
    public Transform groundCheck;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private Collider2D attackHitbox;
    public Vector2 GroundCheckBottomPosition => rb.position + (Vector2)(groundCheck.position - transform.position) + Vector2.down * groundCheckHeight;
    private bool isGrounded;
    private bool jumpPressed;
    private float horizontalInput;
    public float HorizontalInput => horizontalInput;
    public bool slashing;
    private bool slashDamaging;
    private List<Collider2D> slashOverlaps = new();
    private readonly HashSet<EnemyHealth> hitThisSlash = new();
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canMove = true;
    public float GroundCheckWidth => groundCheckwidth;
    public float GroundCheckHeight => groundCheckHeight;

    private void Awake()
    {
        playerAudio = GetComponent<PlayerAudio>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
        groundCheck = transform.Find("GroundCheck");
        attackHitbox = transform.Find("AttackHitbox").GetComponent<Collider2D>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    private void Update()
    {
        horizontalInput = moveAction.ReadValue<Vector2>().x;
        // 先记录跳跃请求，等 FixedUpdate 中与地面状态一起处理。
        if (jumpAction.WasPressedThisFrame())
        {
            jumpPressed = true;
        }

        if (attackAction.WasPressedThisFrame())
        {
            StartSlashing();
        }

        Slash();
    }


    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 处理角色移动的函数
    /// 包括水平移动、跳跃检测和执行
    /// </summary>
    private void Move()
    {
        if (!canMove) return;

        // 检测角色是否接触地面
        // 使用OverlapCircle方法检测角色底部是否在指定图层上
        isGrounded = Physics2D.OverlapBox(
            groundCheck.position,        // 地面检测点的位置
            new Vector2(groundCheckwidth, groundCheckHeight), // 地面检测框的宽高
            0f,                          // 旋转角度
            groundLayer);                // 要检测的地面图层

        // 设置角色的水平速度，保持垂直速度不变
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

        // 如果按下跳跃键且角色在地面上
        if (jumpPressed && isGrounded)
        {
            playerAudio.PlayJump();
            // 给角色一个向上的力，实现跳跃效果
            // ForceMode2D.Impulse表示使用冲量模式，使力瞬间施加
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        // 重置跳跃状态，防止连续跳跃
        jumpPressed = false;
        playerAudio.SetMovePlaying(horizontalInput != 0 && isGrounded);
    }
        private void Slash()
    {
        if (!slashDamaging) return;
        Physics2D.OverlapCollider(attackHitbox, slashOverlaps);
        foreach (Collider2D other  in slashOverlaps)
        {
            EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
            if (enemy != null && hitThisSlash.Add(enemy))
            {
                enemy.TakeDamage(1);
            }
        }
    }



    private void StartSlashing()
    {
        if (!isGrounded || slashing || !canMove || !canAttack) return;
        Debug.Log("start slashing");
        playerAudio.PlaySlash();
        SetCanMove(false);
        hitThisSlash.Clear();
        animator.SetTrigger("Slash");
        slashing = true;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
    public void EndSlashing()
    {
        if (!slashing) return;
        Debug.Log("end slashing");
        slashing = false;
        slashOverlaps.Clear();
        slashDamaging = false;
        SetCanMove(true);
    }
    public void CancelSlashing()
    {
        if (!slashing) return;
        animator.ResetTrigger("Slash");
        slashing = false;
        slashDamaging = false;
        slashOverlaps.Clear();
        hitThisSlash.Clear();
    }
    public void EnableSlashHitbox()
    {
        if (slashing) slashDamaging = true;
    }
    public void DisableSlashHitbox()
    {
        slashDamaging = false;
    }
    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
        public bool GetGrounded()
    {
        return isGrounded;
    }

    // public bool GroundCheckOverlaps(Collider2D targetCollider)
    // {
    //     if (groundCheck == null || targetCollider == null) return false;

    //     Vector2 center = groundCheck.position;
    //     Vector2 closestPoint = targetCollider.ClosestPoint(center);
    //     return (closestPoint - center).sqrMagnitude <= groundCheckwidth * groundCheckwidth + groundCheckHeight * groundCheckHeight;
    // }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckwidth, groundCheckHeight, 0f));
        }
    }
}
