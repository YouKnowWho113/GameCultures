using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuffaloSpam : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    public GameObject cloudA;
    public GameObject cloudS;
    public GameObject cloudD;

    [Header("Cloud Settings")]
    public Transform cloudSpawnPoint; 

    [Header("UI References")]
    public Slider powerBar;
    public RectTransform lineSlow;
    public RectTransform lineFast;
    public RectTransform lineMin;

    [Header("Hint Animation")]
    public Animator hintAnimator; 

    [Header("Power Settings")]
    public float maxPower = 200f;
    public float powerDecay = 10f;
    public float gainPerPress = 15f;

    [Header("Thresholds")]
    public float slowThreshold = 120f;
    public float fastThreshold = 170f;
    public float minThreshold = 90f;

    [Header("Exhaust Settings")]
    public float exhaustDuration = 3f;
    public float overworkTime = 3f;

    [Header("Key Settings")]
    private char[] keyPool = { 'A', 'S', 'D' };
    private char currentKey;
    private float keyChangeTimer;
    private float nextKeyChangeDelay;

    
    private int previousAnimKey = -2;

    
    private float currentPower = 0f;
    private bool isExhausted = false;
    private float exhaustTimer = 0f;
    private float overworkTimer = 0f;

    public float CurrentPower => currentPower;
    public bool IsExhausted => isExhausted;
    public float SlowThreshold => slowThreshold;
    public float FastThreshold => fastThreshold;
    public float MinThreshold => minThreshold;

    public char CurrentKey => currentKey;
    private GameObject activeCloud;

    void Start()
    {
        powerBar.maxValue = maxPower;
        ChooseNewKey();
        UpdateThresholdLines();
    }

    void Update()
    {
        HandleKeyChangeTimer();

        if (isExhausted)
            HandleExhaustion();
        else
        {
            HandleDecay();
            HandleKeyPress();
            HandleOverwork();
        }

        UpdateThresholdLines();
    }

    void HandleDecay()
    {
        if (currentPower > 0)
        {
            currentPower -= powerDecay * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower);
            powerBar.value = currentPower;
        }
    }

    void HandleKeyPress()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(currentKey.ToString().ToLower()) ||
                Input.GetKeyDown(currentKey.ToString().ToUpper()))
            {
                currentPower += gainPerPress;
                currentPower = Mathf.Clamp(currentPower, 0, maxPower);
                powerBar.value = currentPower;
            }
        }
    }

    void HandleOverwork()
    {
        if (currentPower > fastThreshold)
        {
            overworkTimer += Time.deltaTime;
            if (overworkTimer >= overworkTime)
            {
                TriggerExhaustion();
            }
        }
        else
        {
            overworkTimer = 0f;
        }
    }

    void TriggerExhaustion()
    {
        isExhausted = true;
        exhaustTimer = 0f;
        overworkTimer = 0f;

        
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isExhausted", true);
            animator.SetBool("isWalking", false);
        }

        Debug.Log("🔥 Exhaustion Triggered at power: " + currentPower);
    }

    void HandleExhaustion()
    {
        exhaustTimer += Time.deltaTime;

        float drainRate = maxPower / exhaustDuration;
        currentPower -= drainRate * Time.deltaTime;
        currentPower = Mathf.Clamp(currentPower, 0, maxPower);
        powerBar.value = currentPower;

        if (exhaustTimer >= exhaustDuration)
        {
            isExhausted = false;
            currentPower = 0f;
            powerBar.value = currentPower;

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("isExhausted", false);
            }

            Debug.Log("✅ Buffalo recovered");
        }
    }

    void HandleKeyChangeTimer()
    {
        keyChangeTimer += Time.deltaTime;
        if (keyChangeTimer >= nextKeyChangeDelay)
        {
            ChooseNewKey();
        }
    }

    void ChooseNewKey()
    {
        char newKey;
        do
        {
            newKey = keyPool[Random.Range(0, keyPool.Length)];
        } while (newKey == currentKey);

        currentKey = newKey;

        

        
        keyChangeTimer = 0f;
        nextKeyChangeDelay = Random.Range(5f, 7f);

        
        SpawnCloud(currentKey);

        
    }

    void SpawnCloud(char key)
    {
        
        if (activeCloud != null)
        {
            Destroy(activeCloud);
        }

        GameObject prefab = null;

        if (key == 'A') prefab = cloudA;
        else if (key == 'S') prefab = cloudS;
        else if (key == 'D') prefab = cloudD;

        if (prefab != null && cloudSpawnPoint != null)
        {
            
            activeCloud = Instantiate(prefab, cloudSpawnPoint.position, Quaternion.identity);
        }
    }


    

    void UpdateThresholdLines()
    {
        if (lineSlow == null || lineFast == null || lineMin == null || powerBar == null) return;

        float barWidth = ((RectTransform)powerBar.fillRect.parent).rect.width;

        float slowNorm = slowThreshold / maxPower;
        float fastNorm = fastThreshold / maxPower;
        float minNorm = minThreshold / maxPower;

        lineSlow.anchoredPosition = new Vector2(slowNorm * barWidth, lineSlow.anchoredPosition.y);
        lineFast.anchoredPosition = new Vector2(fastNorm * barWidth, lineFast.anchoredPosition.y);
        lineMin.anchoredPosition = new Vector2(minNorm * barWidth, lineMin.anchoredPosition.y);
    }
}