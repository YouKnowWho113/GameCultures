using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BuffaloStats : MonoBehaviour
{
    public static BuffaloStats Instance { get; private set; }

    [Header("UI")]
    public Slider growthBar;
    public string barName = "Growth Bar";

    [Header("Values")]
    public float maxGrowth = 100f;
    public float currentGrowth = 0f;

    [Header("Nutrition")]
    public float nutritionFresh = 15f;
    public float nutritionRotten = -10f;

    private bool hasTriggered = false;
    private float timer = 0f;
    private float timeLimit = 15f;

    void Awake()
    {
        
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (growthBar != null)
        {
            growthBar.minValue = 0f;
            growthBar.maxValue = maxGrowth;
            growthBar.value = currentGrowth;
        }
    }

    void Update()
    {
        if (!hasTriggered)
        {
            timer += Time.deltaTime;
            if (timer >= timeLimit)
            {
                hasTriggered = true;
                LoadNextDay();
            }
        }
    }

    public void FeedCorn(DraggableCorn corn)
    {
        if (corn == null || corn.consumed) return;

        float delta = corn.isFresh ? nutritionFresh : nutritionRotten;
        currentGrowth = Mathf.Clamp(currentGrowth + delta, 0f, maxGrowth);

        if (growthBar != null) growthBar.value = currentGrowth;

        corn.MarkConsumed();

        if (!hasTriggered && currentGrowth >= maxGrowth)
        {
            hasTriggered = true;
            LoadNextDay();
        }
    }

    void LoadNextDay()
    {
        Debug.Log("🌱 Buffalo fully grown or time's up! Loading Day 2...");
        SceneManager.LoadScene("Day2");
    }

    
    public void ResetGrowth()
    {
        currentGrowth = 0f;
        timer = 0f;
        hasTriggered = false;

        if (growthBar != null) growthBar.value = currentGrowth;
    }
}