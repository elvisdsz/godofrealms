using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Typer : MonoBehaviour
{
    public TextMeshProUGUI wordTextUI;
    private string word;
    private string remainingWord;
    private bool inCircleFlag=false;
    private float timeSinceOverlap;
    private float timeToHideText = 1f;

    public void Init(string word)
    {
        word = word.ToLower();
        this.word = word;
        remainingWord = word;
        timeSinceOverlap = 10f;
        inCircleFlag = false;
    }

    public void TyperUpdate()
    {
        if(inCircleFlag) {
            timeSinceOverlap += Time.deltaTime;
            if(timeSinceOverlap >= timeToHideText)
            {
                inCircleFlag=false;
                // Reset remaining word after leaving circle
                remainingWord = word;
            }
            CheckKeyPressed();
        }
        
        RefreshUI();
    }

    public void InCircle()
    {
        timeSinceOverlap = 0f;
        inCircleFlag = true;
    }

    private void CheckKeyPressed()
    {
        if(Input.anyKeyDown)
        {
            // Use string for easy conversion to lowercase
            string keyPressed = Input.inputString;
            // Only take input string that only has one letter
            if(keyPressed.Length == 1)
                EnterLetter(keyPressed);
        }
    }

    public void EnterLetter(string letter)
    {
        if(remainingWord.Length == 0)
            return;

        if(remainingWord[0] == letter.ToLower()[0])
        {
            remainingWord = remainingWord.Remove(0, 1);
            RefreshUI();
            if(remainingWord.Length == 0)
                WordCompleted();
        }
    }

    public void RefreshUI()
    {
        wordTextUI.text = remainingWord;
        wordTextUI.enabled = inCircleFlag;
    }

    public abstract void WordCompleted();

}
