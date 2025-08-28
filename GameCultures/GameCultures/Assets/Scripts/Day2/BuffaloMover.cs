using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffaloMover : MonoBehaviour
{
    public BuffaloSpam spamScript;
    public float slowSpeed = 2f;
    public float fastSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (spamScript == null) return;

        if (spamScript.IsExhausted)
        {
            // Don’t move
            rb.velocity = Vector2.zero;
            return;
        }

        float power = spamScript.CurrentPower;

        if (power < spamScript.SlowThreshold)
        {
            rb.velocity = new Vector2(slowSpeed, rb.velocity.y);  // move slowly
        }
        else if (power <= spamScript.FastThreshold)
        {
            rb.velocity = new Vector2(fastSpeed, rb.velocity.y);  // max speed
        }
    }
}
