using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffaloMover : MonoBehaviour
{
    public BuffaloSpam spamScript;
    public float slowSpeed = 2f;
    public float fastSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (spamScript == null) return;

        if (spamScript.IsExhausted)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isExhausted", true);
            animator.SetBool("isWalking", false);
            return;
        }
        else
        {
            animator.SetBool("isExhausted", false);
        }

        float power = spamScript.CurrentPower;

        if (power < spamScript.MinThreshold) 
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("isWalking", false);
        }
        else if (power < spamScript.SlowThreshold) 
        {
            rb.velocity = new Vector2(slowSpeed, rb.velocity.y);
            animator.SetBool("isWalking", true);
        }
        else if (power <= spamScript.FastThreshold) 
        {
            rb.velocity = new Vector2(fastSpeed, rb.velocity.y);
            animator.SetBool("isWalking", true);
        }
    }
}