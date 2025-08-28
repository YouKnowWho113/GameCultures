using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuffaloSpam : MonoBehaviour
{
    [Header("UI References")]
    public Text keyText;
    public Slider powerBar;
    public RectTransform lineSlow;   
    public RectTransform lineFast;   
    public RectTransform lineMin;

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

   
    private float currentPower = 0f;
    private bool isExhausted = false;
    private float exhaustTimer = 0f;
    private float overworkTimer = 0f;

    public float CurrentPower => currentPower;
    public bool IsExhausted => isExhausted;
    public float SlowThreshold => slowThreshold;
    public float FastThreshold => fastThreshold;

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
        Debug.Log("Buffalo is exhausted! Needs to rest...");
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
            overworkTimer = 0f;
            Debug.Log("Buffalo recovered!");
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
        currentKey = keyPool[Random.Range(0, keyPool.Length)];
        keyText.text = "Press: " + currentKey;

        keyChangeTimer = 0f;
        nextKeyChangeDelay = Random.Range(5f, 7f);
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