using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private RealmManager realm;

    private Rigidbody2D playerRigidbody;
    public float speed;
    public float speedCoefficient;

    public PowerupData powerupData = new PowerupData();

    public Sprite playerUpSprite;
    public Sprite playerDownSprite;
    public Sprite playerLeftSprite;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0f;
        float y = 0f;

        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
        }

        UpdatePlayerSprite(x, y);

        // Speed coef range from 1 to 2
        speedCoefficient = 1f + powerupData.PowerupValue(PowerupData.PowerupType.SPEED_UP);

        playerRigidbody.AddForce(new Vector2(x,y) * speed * speedCoefficient * Time.deltaTime);

        /*if(realm!=null && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 colliderPosition = transform.position + new Vector3(0,-transform.localScale.y,0);
            realm.Hit(colliderPosition);
        }*/
    }

    public void UpdatePlayerSprite(float x, float y)
    {
        if(x==0f && y==0f)
            return;
        
        if(Mathf.Abs(x) > Mathf.Abs(y))
        {
            spriteRenderer.sprite = playerLeftSprite;
            if(x<=0)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
            if(y<=0)
                spriteRenderer.sprite = playerDownSprite;
            else
                spriteRenderer.sprite = playerUpSprite;
        }
    }

    public void SetNewRealm(RealmManager realm) {
        this.realm = realm;
        Debug.Log("Entering new realm -- "+realm.name);
    }

    public void ResetRealm(RealmManager realm) {
        if(this.realm == realm)
            this.realm = null;
        
        Debug.Log("Exiting realm -- "+realm.name);
    }

    public RealmManager GetRealm()
    {
        return this.realm;
    }
    
}
