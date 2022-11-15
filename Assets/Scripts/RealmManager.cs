using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealmManager : MonoBehaviour
{
    public Color realmColor;
    public int maxHits = 10;
    private int currentHits = 0;
    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        if(currentHits >= maxHits)
            return;
        
        currentHits+=1;
        tilemap.color = Color.Lerp(Color.white, realmColor, (float)currentHits/maxHits);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            PlayerMovement player = collider.gameObject.GetComponent<PlayerMovement>();
            player.SetNewRealm(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            PlayerMovement player = collider.gameObject.GetComponent<PlayerMovement>();
            player.ResetRealm(gameObject);
        }
    }
}
