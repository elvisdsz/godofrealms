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

    private bool released = false;

    // Start is called before the first frame update
    void Start()
    {
        soulRigidbody = GetComponent<Rigidbody2D>();
        Init("PEACE", realm); // test-only
    }

    public void Init(string word, RealmManager realm)
    {
        timeSinceSpawn = 0f;
        word = word.ToLower();
        remainingWord = word;
        timeSinceOverlap = 10f;
        inCircleFlag = false;
        this.realm = realm;
        soulRigidbody = GetComponent<Rigidbody2D>();    //Redundant but required
        SetRandomVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        if(released)
            return;
            
        timeSinceSpawn += Time.deltaTime;
        if(inCircleFlag) {
            timeSinceOverlap += Time.deltaTime;
            if(timeSinceOverlap >= timeToHideText)
            {
                inCircleFlag=false;
                // Reset remaining word after leaving circle
                remainingWord = word;
            }
        }
        
        RefreshUI();

        Wander(); // soul movement

        if(inCircleFlag)
        {
            CheckKeyPressed();
        }

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

    private void SetRandomVelocity()
    {
        float velocityX = Random.Range(-1f, 1f);
        float velocityY = Random.Range(-1f, 1f);
        soulRigidbody.velocity = new Vector2(velocityX, velocityY).normalized * wanderSpeed;
    }

    public void EnterLetter(string letter)
    {
        if(remainingWord.Length == 0)
        {
            if(!released)
                ReleaseSoul();
            return;
        }

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
        released = true;
        soulRigidbody.velocity = Vector2.zero;
        realm.RemoveSoulFromRealm(this, transform.position); // FIXME - player position?
    }

}
