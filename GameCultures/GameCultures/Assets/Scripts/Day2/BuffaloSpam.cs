using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuffaloSpam : MonoBehaviour
{
    [Header("UI References")]
    public Text keyText;
    public Slider powerBar;
    public RectTransform lineSlow;   // ExhaustBorder
    public RectTransform lineFast;   // MaxBorder
    public RectTransform lineMin;

    [Header("Power Settings")]
    public float maxPower = 200f;
    public float powerDecay = 10f;   // loses power per second
    public float gainPerPress = 15f; // gain per correct key press

    [Header("Thresholds")]
    public float slowThreshold = 120f;
    public float fastThreshold = 170f;
    public float minThreshold = 90f;

    [Header("Exhaust Settings")]
    public float exhaustDuration = 3f;  // how long buffalo rests
    public float overworkTime = 3f;     // how long above fastThreshold before exhaustion

    [Header("Key Settings")]
    private char[] keyPool = { 'A', 'S', 'D' };
    private char currentKey;
    private float keyChangeTimer;
    private float nextKeyChangeDelay;

    // --- Internal state ---
    private float currentPower = 0f;
    private bool isExhausted = false;
    private float exhaustTimer = 0f;
    private float overworkTimer = 0f;

    // Public properties (getter only)
    public float CurrentPower => currentPower;
    public bool IsExhausted => isExhausted;
    public float SlowThreshold => slowThreshold;
    public float FastThreshold => fastThreshold;

    void Start()
    {
        powerBar.maxValue = maxPower;
        ChooseNewKey();
        UpdateThresholdLines(); // Initialize line positions at start
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

        UpdateThresholdLines(); // keep updating line positions
    }

    // --- Power Decay ---
    void HandleDecay()
    {
        if (currentPower > 0)
        {
            currentPower -= powerDecay * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower);
            powerBar.value = currentPower;
        }
    }

    // --- Key Press ---
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

    // --- Overwork Check ---
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
            overworkTimer = 0f; // reset if back in safe zone
        }
    }

    // --- Exhaustion ---
    void TriggerExhaustion()
    {
        isExhausted = true;
        exhaustTimer = 0f;
        Debug.Log("Buffalo is exhausted! Needs to rest...");
    }

    void HandleExhaustion()
    {
        exhaustTimer += Time.deltaTime;

        // Smoothly drain to 0 over exhaustDuration
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

    // --- Random Key Switching ---
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

    // --- Threshold Lines Update ---
    void UpdateThresholdLines()
    {
        if (lineSlow == null || lineFast == null || lineMin == null || powerBar == null) return;

        // Get the width of the bar
        float barWidth = ((RectTransform)powerBar.fillRect.parent).rect.width;

        // Normalize thresholds
        float slowNorm = slowThreshold / maxPower;
        float fastNorm = fastThreshold / maxPower;
        float minNorm = minThreshold / maxPower;

        // Apply anchored position relative to left edge
        lineSlow.anchoredPosition = new Vector2(slowNorm * barWidth, lineSlow.anchoredPosition.y);
        lineFast.anchoredPosition = new Vector2(fastNorm * barWidth, lineFast.anchoredPosition.y);
        lineMin.anchoredPosition = new Vector2(minNorm * barWidth, lineMin.anchoredPosition.y);
    }
}