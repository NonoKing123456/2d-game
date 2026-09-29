using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Math.Abs(GetComponent<Rigidbody2D>().linearVelocity.x));
        if (GetComponent<Rigidbody2D>().linearVelocity.x > 0.1 || GetComponent<Rigidbody2D>().linearVelocity.x < -0.1)
        {
            transform.localScale = new Vector3(Math.Sign(GetComponent<Rigidbody2D>().linearVelocity.x), 1, 1);
        }
        animator.SetBool("Grounded", GetComponent<PlayerControl>().GetGrounded());
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
    }
}
