using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffaloStats : MonoBehaviour
{
    public static BuffaloStats Instance { get; private set; }

    [Header("UI")]
    public Slider growthBar;            // link your Slider in inspector
    public string barName = "Growth Bar";

    [Header("Values")]
    public float maxGrowth = 100f;
    public float currentGrowth = 0f;

    [Header("Nutrition")]
    public float nutritionFresh = 15f;
    public float nutritionRotten = -10f; // set 0 if you prefer no penalty

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

    public void FeedCorn(DraggableCorn corn)
    {
        if (corn == null || corn.consumed) return;

        float delta = corn.isFresh ? nutritionFresh : nutritionRotten;
        currentGrowth = Mathf.Clamp(currentGrowth + delta, 0f, maxGrowth);

        if (growthBar != null) growthBar.value = currentGrowth;

        corn.MarkConsumed();
    }
}