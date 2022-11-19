using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private RealmManager realm;

    private Rigidbody2D playerRigidbody;
    private float speed = 3f;

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

        playerRigidbody.AddForce(new Vector2(x,y) * speed);

        /*if(realm!=null && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 colliderPosition = transform.position + new Vector3(0,-transform.localScale.y,0);
            realm.Hit(colliderPosition);
        }*/
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
