using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornTree : MonoBehaviour
{
    [Header("Corn Prefabs")]
    public GameObject freshCornPrefab;
    public GameObject rottenCornPrefab;

    [Header("Tree Sprites")]
    public Sprite readySprite;
    public Sprite cooldownSprite;

    [Header("Cooldown Settings")]
    public float cooldownSeconds = 5f;

    private bool isOnCooldown = false;
    private SpriteRenderer sr;


    private int cornCount = 0;

    [Header("Sound")]
    public AudioClip pickupSound;
    private AudioSource audioSource;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (sr != null)
        {
            if (readySprite != null)
            {
                sr.sprite = readySprite;
            }
            else if (sr.sprite == null)
            {
                Debug.LogWarning("Tree has no sprite assigned!");
            }
        }
    }
    
    void OnMouseDown()
    {
        if (isOnCooldown) return;

        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }


        GameObject prefabToSpawn = (Random.value > 0.5f) ? freshCornPrefab : rottenCornPrefab;
        if (prefabToSpawn == null)
        {
            Debug.LogError("Corn prefab is not assigned!");
            return;
        }

        
        Vector3 spawnPos = transform.position + new Vector3(0, 1.5f, 0);
        spawnPos.z = 0f;
        GameObject corn = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        Debug.Log("Spawned corn: " + corn.name);

        
        var cornSr = corn.GetComponent<SpriteRenderer>();
        if (cornSr != null)
        {
            cornSr.sortingLayerName = "Default";
            cornSr.sortingOrder = 999;
            cornSr.color = Color.white;
        }

        
        Collider2D cornCol = corn.GetComponent<Collider2D>();
        if (cornCol != null)
        {
            cornCol.enabled = false;
            StartCoroutine(EnableColliderNextFrame(cornCol));
        }

        
        if (sr != null && cooldownSprite != null)
        {
            sr.sprite = cooldownSprite;
        }

        StartCoroutine(CooldownRoutine());
    }

    IEnumerator EnableColliderNextFrame(Collider2D col)
    {
        yield return null;
        col.enabled = true;
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;

        
        yield return new WaitForSeconds(cooldownSeconds);

        
        while (cornCount > 0)
        {
            yield return null;
        }

        isOnCooldown = false;
        if (sr != null && readySprite != null)
        {
            sr.sprite = readySprite;
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Corn"))
        {
            cornCount++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Corn"))
        {
            cornCount = Mathf.Max(0, cornCount - 1);
        }
    }
}