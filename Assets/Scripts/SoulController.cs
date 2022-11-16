using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulController : MonoBehaviour
{
    public TextMeshProUGUI wordTextUI;

    private string word = "WORD";
    private string remainingWord;
    private float timeSinceSpawn;
    [SerializeField] private RealmManager realm;
    private float timeSinceOverlap;
    private bool inCircleFlag=false;
    private float wanderSpeed = 1.2f;

    private float timeToHideText = 1f;

    private Rigidbody2D soulRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        soulRigidbody = GetComponent<Rigidbody2D>();
        Init("PEACE"); // test-only
    }

    void Init(string word)
    {
        timeSinceSpawn = 0f;
        word = word.ToLower();
        remainingWord = word;
        timeSinceOverlap = 10f;
        inCircleFlag = false;
        SetRandomVelocity();
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

        Wander(); // soul movement

        // TODO: soul stays in the same realm
    }

    private void Wander()
    {
        Vector2 forwardPosition = (Vector2)transform.position + (soulRigidbody.velocity);

        for(int i=1; i<10; i++) {
            if(realm.IsPositionOnTilemap(forwardPosition))
                break;

            SetRandomVelocity();

            Debug.DrawRay(transform.position, soulRigidbody.velocity/2f, Color.green, 2f);
            forwardPosition = (Vector2)transform.position + (soulRigidbody.velocity/2f);
        }
    
    }

    private void SetRandomVelocity()
    {
        float velocityX = Random.Range(-1f, 1f);
        float velocityY = Random.Range(-1f, 1f);
        soulRigidbody.velocity = new Vector2(velocityX, velocityY).normalized * wanderSpeed;
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
