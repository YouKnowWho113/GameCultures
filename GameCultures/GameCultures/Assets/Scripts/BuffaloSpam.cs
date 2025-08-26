using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffaloSpam : MonoBehaviour
{
    public TMP_Text keyText;       
    public Slider powerBar;        
    public float maxPower = 100f; 
    public float powerDecay = 5f;  
    public float gainPerPress = 10f; 

    private char currentKey;
    private float currentPower = 0f;

    void Start()
    {
        powerBar.maxValue = maxPower;
        ChooseNewKey();
    }

    void Update()
    {
        
        if (currentPower > 0)
        {
            currentPower -= powerDecay * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower);
            powerBar.value = currentPower;
        }

        
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(currentKey.ToString().ToLower()) ||
                Input.GetKeyDown(currentKey.ToString().ToUpper()))
            {
                currentPower += gainPerPress;
                powerBar.value = currentPower;

                if (currentPower >= maxPower)
                {
                    Debug.Log("Buffalo powered up! Next key...");
                    ChooseNewKey();
                }
            }
        }
    }

    void ChooseNewKey()
    {
       
        currentKey = (char)('A' + Random.Range(0, 26));
        keyText.text = "Press: " + currentKey;
        currentPower = 0;
        powerBar.value = currentPower;
    }
}