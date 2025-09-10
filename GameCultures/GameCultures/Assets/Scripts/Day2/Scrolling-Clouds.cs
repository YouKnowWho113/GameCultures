using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingClouds : MonoBehaviour
{
    public float xLimit = 0f;
    public Vector3 startPos;
    public float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        if (transform.position.x >= xLimit)
        {
            transform.position = startPos;
        }
    }
}
