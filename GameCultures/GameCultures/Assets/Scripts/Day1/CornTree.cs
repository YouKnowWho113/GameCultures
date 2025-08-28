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

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

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

        
        GameObject prefabToSpawn = (Random.value > 0.6f) ? freshCornPrefab : rottenCornPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError("Corn prefab is not assigned!");
            return;
        }

        
        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);
        spawnPos.z = 0f;
        GameObject corn = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        Debug.Log("Spawned corn: " + corn.name);

        
        var cornSr = corn.GetComponent<SpriteRenderer>();
        if (cornSr != null)
        {
            if (cornSr.sprite == null)
            {
                Debug.LogError("Corn prefab has no sprite assigned!");
            }

            
            cornSr.sortingLayerName = "Default";
            cornSr.sortingOrder = 999;
            cornSr.color = Color.white;
        }
        else
        {
            Debug.LogError("Corn prefab missing SpriteRenderer!");
        }

        
        if (sr != null && cooldownSprite != null)
        {
            sr.sprite = cooldownSprite;
        }

        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownSeconds);

        isOnCooldown = false;
        if (sr != null && readySprite != null)
        {
            sr.sprite = readySprite;
        }
    }
}