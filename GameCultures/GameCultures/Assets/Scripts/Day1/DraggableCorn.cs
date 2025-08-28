﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class DraggableCorn : MonoBehaviour
{
    [Header("Corn Type")]
    public bool isFresh = true; 

    [Header("Pickup")]
    public float holdToPickSeconds = 0.15f;
    public float pickedScale = 1.15f;

 
    public bool IsBeingCarried { get; private set; }
    public bool consumed { get; private set; }

    float holdTimer;
    bool isHoldingMouse;
    Vector3 originalScale;

    Rigidbody2D rb;

    void Start()
    {
        originalScale = transform.localScale;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
        }
    }

    void OnMouseDown()
    {
        if (consumed) return;

        isHoldingMouse = true;
        holdTimer = 0f;
    }

    void OnMouseUp()
    {
        if (consumed) return;

        isHoldingMouse = false;

        if (IsBeingCarried)
        {
            IsBeingCarried = false;
            transform.localScale = originalScale;

            
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    void Update()
    {
        if (consumed) return;

        if (isHoldingMouse && !IsBeingCarried)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdToPickSeconds)
            {
                PickUp();
            }
        }

        if (IsBeingCarried && Input.GetMouseButton(0))
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            world.z = 0f;
            transform.position = world;
        }
    }

    void PickUp()
    {
        IsBeingCarried = true;
        isHoldingMouse = true; 
        transform.localScale = originalScale * pickedScale;

        
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void AutoPick()
    {
        if (consumed) return;
        holdTimer = holdToPickSeconds; 
        PickUp();
    }

    public void MarkConsumed()
    {
        if (consumed) return;
        consumed = true;
        Destroy(gameObject);
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsBeingCarried && collision.gameObject.CompareTag("Ground"))
        {
            MarkConsumed();
        }
    }
}