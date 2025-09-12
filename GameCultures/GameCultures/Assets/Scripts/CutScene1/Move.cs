using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 2f; 
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
