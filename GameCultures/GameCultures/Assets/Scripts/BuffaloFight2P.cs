using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffaloFight2P : MonoBehaviour
{
    public TMP_Text p1SequenceText; 
    public TMP_Text p2SequenceText; 
    public int sequenceLength = 3; 

    private List<KeyCode> currentSequence = new List<KeyCode>();
    private int currentIndexP1 = 0;
    private int currentIndexP2 = 0;
    private int round = 1;
    private bool roundOver = false;

    

    

    void Start()
    {
        GenerateSequence();
    }

    void GenerateSequence()
    {
        currentSequence.Clear();

        KeyCode[] arrows = { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };

        for (int i = 0; i < sequenceLength; i++)
        {
            KeyCode arrow = arrows[Random.Range(0, arrows.Length)];
            currentSequence.Add(arrow);
        }

        currentIndexP1 = 0;
        currentIndexP2 = 0;
        roundOver = false;
        UpdateSequenceText();
    }

    void Update()
    {
        if (!roundOver)
        {
            HandlePlayer1Input();
            HandlePlayer2Input();
        }
    }

    void HandlePlayer1Input()
    {
        if (currentIndexP1 < currentSequence.Count)
        {
            if (GetP1Input(currentSequence[currentIndexP1]))
            {
                currentIndexP1++;
                UpdateSequenceText();

                if (currentIndexP1 == currentSequence.Count)
                {
                    Debug.Log("Player 1 finished first!");
                    Debug.Log("Minus health2");
                    roundOver = true;
                    NextRound();
                }
            }
            else if (AnyP1KeyDown() && !GetP1Input(currentSequence[currentIndexP1]))
            {
                Debug.Log("P1 Wrong Key! Reset progress");
                currentIndexP1 = 0;
                UpdateSequenceText();
            }
        }
    }

    void HandlePlayer2Input()
    {
        if (currentIndexP2 < currentSequence.Count)
        {
            if (GetP2Input(currentSequence[currentIndexP2]))
            {
                currentIndexP2++;
                UpdateSequenceText();

                if (currentIndexP2 == currentSequence.Count)
                {
                    Debug.Log("Player 2 finished first!");
                    roundOver = true;
                    NextRound();
                }
            }
            else if (AnyP2KeyDown() && !GetP2Input(currentSequence[currentIndexP2]))
            {
                Debug.Log("P2 Wrong Key! Reset progress");
                currentIndexP2 = 0;
                UpdateSequenceText();
            }
        }
    }

    
    bool GetP1Input(KeyCode expected)
    {
        if (expected == KeyCode.UpArrow && Input.GetKeyDown(KeyCode.W)) return true;
        if (expected == KeyCode.DownArrow && Input.GetKeyDown(KeyCode.S)) return true;
        if (expected == KeyCode.LeftArrow && Input.GetKeyDown(KeyCode.A)) return true;
        if (expected == KeyCode.RightArrow && Input.GetKeyDown(KeyCode.D)) return true;
        return false;
    }

    
    bool GetP2Input(KeyCode expected)
    {
        return Input.GetKeyDown(expected);
    }

    bool AnyP1KeyDown()
    {
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
               Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);
    }

    bool AnyP2KeyDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
               Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow);
    }

    void UpdateSequenceText()
    {
        
        p1SequenceText.text = GetProgressString(currentIndexP1, true);  
        p2SequenceText.text = GetProgressString(currentIndexP2, false); 
    }

    string GetProgressString(int index, bool forPlayer1)
    {
        string result = "";
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (i < index)
            {
                result += " "; 
            }
            else
            {
                result += (forPlayer1 ? ArrowToWASD(currentSequence[i]) : ArrowToSymbol(currentSequence[i])) + " ";
            }
        }
        return result;
    }

    
    string ArrowToWASD(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
        }
        return "";
    }

    
    string ArrowToSymbol(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow: return "↑";
            case KeyCode.DownArrow: return "↓";
            case KeyCode.LeftArrow: return "←";
            case KeyCode.RightArrow: return "→";
        }
        return "";
    }

    void NextRound()
    {
        round++;

        // Harder every 3 rounds, max 6
        if (round % 3 == 0 && sequenceLength < 6)
        {
            sequenceLength++;
        }

        GenerateSequence();
    }
}