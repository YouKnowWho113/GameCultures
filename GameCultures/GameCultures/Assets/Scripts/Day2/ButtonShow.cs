using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonShow : MonoBehaviour
{
    public Animator anim;
    private char currentKey;
    private char[] keyPool = { 'A', 'S', 'D' };
    private float keyChangeTimer = 0f;
    private float nextKeyChangeDelay;

    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        ChooseNewKey();
    }

    void Update()
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

        if (anim != null)
        {
            if (currentKey == 'A') anim.SetInteger("CurrentKey", 0);
            else if (currentKey == 'S') anim.SetInteger("CurrentKey", 1);
            else if (currentKey == 'D') anim.SetInteger("CurrentKey", 2);
        }

        keyChangeTimer = 0f;
        nextKeyChangeDelay = Random.Range(5f, 7f);
    }

    public char CurrentKey => currentKey;
}
