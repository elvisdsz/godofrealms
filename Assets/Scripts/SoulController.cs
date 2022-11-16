using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulController : MonoBehaviour
{
    public TextMeshProUGUI wordTextUI;

    private string word;
    private string remainingWord;
    private float timeSinceSpawn;
    private RealmManager realm;
    private float timeSinceOverlap;
    private bool inCircleFlag;

    private float timeToHideText = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SetWord("PEACE"); // test-only
        Init();
    }

    void Init()
    {
        timeSinceSpawn = 0f;
        word = word.ToLower();
        remainingWord = word;
        timeSinceOverlap = 10f;
        inCircleFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if(inCircleFlag) {
            timeSinceOverlap += Time.deltaTime;
            if(timeSinceOverlap >= timeToHideText)
                inCircleFlag=false;
        }
        
        RefreshUI();

        // TODO: soul stays in the same realm
    }

    public void EnterLetter(string letter)
    {
        if(remainingWord[0] == letter.ToLower()[0])
        {
            remainingWord = remainingWord.Remove(0, 1);
            RefreshUI();
            if(remainingWord.Length == 0)
                ReleaseSoul();
        }
    }

    public void InCircle()
    {
        timeSinceOverlap = 0f;
        inCircleFlag = true;
    }

    public void SetWord(string word)
    {
        this.word = word;
    }

    public void RefreshUI()
    {
        wordTextUI.text = remainingWord;
        wordTextUI.enabled = inCircleFlag;
    }

    private void ReleaseSoul()
    {
        // TODO
    }

}
