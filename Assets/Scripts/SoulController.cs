using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulController : Typer
{
    private float timeSinceSpawn;
    [SerializeField] private RealmManager realm;
    private float wanderSpeed = 1.2f;

    private Rigidbody2D soulRigidbody;

    private bool released = false;

    private PlayerMovement playerMovement;

    [SerializeField] private bool tutorialSoul;

    // Start is called before the first frame update
    void Start()
    {
        soulRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if(tutorialSoul) {
            base.Init("RELEASE");
        }
    }

    public void Init(string word, RealmManager realm)
    {
        timeSinceSpawn = 0f;
        this.realm = realm;
        soulRigidbody = GetComponent<Rigidbody2D>();    //Redundant but required
        SetRandomVelocity();
        base.Init(word);
    }

    // Update is called once per frame
    void Update()
    {
        if(released)
            return;

        timeSinceSpawn += Time.deltaTime;

        if(!tutorialSoul)
            Wander(); // soul movement

        TyperUpdate();
    }

    private void Wander()
    {
        if(soulRigidbody.velocity == Vector2.zero)
        {
            SetRandomVelocity();
            return;
        }

        Vector2 forwardPosition = (Vector2)transform.position + (soulRigidbody.velocity/2f);

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

    /*public void SetWord(string word)
    {
        this.word = word;
    }*/

    private void ReleaseSoul()
    {
        released = true;
        soulRigidbody.velocity = Vector2.zero;
        if(!tutorialSoul)
            realm.RemoveSoulFromRealm(this, transform.position);
        else {
            Vector3Int gridPosition = playerMovement.GetRealm().GetPositionOnTilemap(transform.position);
            StartCoroutine(playerMovement.GetRealm().Blast(gridPosition));
            GameObject.Destroy(gameObject, 1f);
            playerMovement.GetRealm().acquired = true;
        }
    }

    public override void WordCompleted()
    {
        ReleaseSoul();
    }

    public override void OnCorrectKeyPress()
    {
        /*if(playerMovement.powerupData.IsPowerupEnabled(PowerupData.PowerupType.ATTRACT_SOUL))
        {
            Vector2 velocity = playerMovement.transform.position - transform.position;
            soulRigidbody.velocity = velocity;
        }*/
        if(playerMovement.powerupData.PowerupValue(PowerupData.PowerupType.ATTRACT_SOUL) > 0f)
        {
            float attractCoeff = playerMovement.powerupData.PowerupValue(PowerupData.PowerupType.ATTRACT_SOUL);
            Vector2 velocity = (playerMovement.transform.position - transform.position) * attractCoeff;
            soulRigidbody.velocity = velocity;
        }
    }
}
