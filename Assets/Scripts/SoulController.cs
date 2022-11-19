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

    // Start is called before the first frame update
    void Start()
    {
        soulRigidbody = GetComponent<Rigidbody2D>();
        //Init("PEACE", realm); // test-only
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

        Wander(); // soul movement

        TyperUpdate();
    }

    private void Wander()
    {
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
        realm.RemoveSoulFromRealm(this, transform.position);
    }

    public override void WordCompleted()
    {
        ReleaseSoul();
    }
}
