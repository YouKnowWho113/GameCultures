using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FInishPoint : MonoBehaviour
{
    public float restartAfterSeconds = 30f;
    private float timer = 0f;

    void Update()
    {
        
        timer += Time.deltaTime;

     
        if (timer >= restartAfterSeconds)
        {
            SceneManager.LoadScene("Day2");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player") || other.CompareTag("Buffalo"))
        {
            SceneManager.LoadScene("Day2");
        }
    }
}

    
