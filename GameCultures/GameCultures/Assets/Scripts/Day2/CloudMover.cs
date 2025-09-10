using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudMover : MonoBehaviour
{
    public float speed = 2f;
    public float destroyX = 20f; 

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > destroyX)
        {
            Destroy(gameObject);
        }
    }
    
}
