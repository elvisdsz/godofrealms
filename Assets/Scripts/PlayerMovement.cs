using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public GameObject tilemapObj;

    private Rigidbody2D rb;

    private bool blastOn = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        rb.AddForce(new Vector2(x,y));

        if(!blastOn && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(Blast());
        }
    }

    IEnumerator Blast()
    {
        Vector3 colliderPosition = transform.position + new Vector3(0,-transform.localScale.y,0);

        if(tilemapObj != null) {
            blastOn = true;
            yield return null;

            GridLayout grid = tilemapObj.GetComponentInParent<GridLayout>();
            Vector3Int gridPosition = grid.WorldToCell(colliderPosition);

            // Debug.Log("Blast at "+ gridPosition);

            Tilemap tilemap = tilemapObj.GetComponent<Tilemap>();
            // TileBase tile = tilemap.GetTile(gridPosition);

            RealmManager realmManager = tilemapObj.GetComponent<RealmManager>();
            realmManager.Hit();
            
            ChangeTileColor(tilemap, gridPosition, realmManager.realmColor);
            yield return new WaitForSeconds(0.5f);
            ChangeTileColor(tilemap, gridPosition, Color.white);

            blastOn = false;
        }
    }

    private void ChangeTileColor(Tilemap tilemap, Vector3Int position, Color color) {
        Vector3Int[] positionArr = {position, position+Vector3Int.up, position+Vector3Int.down, position+Vector3Int.right, position+Vector3Int.left,
            position+Vector3Int.up+Vector3Int.up+Vector3Int.right+Vector3Int.right, position+Vector3Int.up+Vector3Int.up+Vector3Int.left+Vector3Int.left,
            position+Vector3Int.down+Vector3Int.down+Vector3Int.right+Vector3Int.right, position+Vector3Int.down+Vector3Int.down+Vector3Int.left+Vector3Int.left,
        };

        for(int i=0; i<positionArr.Length; i++) {
            Vector3Int ipos = positionArr[i];
            if(tilemap.HasTile(position)) {
                tilemap.SetTileFlags(ipos, TileFlags.None);
                tilemap.SetColor(ipos, color);
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Realm") && collider.gameObject != tilemapObj)
        {
            tilemapObj = collider.gameObject;
            Debug.Log("Entering new realm -- "+tilemapObj.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Realm"))
        {
            tilemapObj = null;
            Debug.Log("Exiting realm -- "+collider.gameObject.name);
        }
    }*/

    public void SetNewRealm(GameObject tilemapObject) {
        this.tilemapObj = tilemapObject;
        Debug.Log("Entering new realm -- "+tilemapObj.name);
    }

    public void ResetRealm(GameObject tilemapObject) {
        if(this.tilemapObj == tilemapObject)
            this.tilemapObj = null;
        
        Debug.Log("Exiting realm -- "+tilemapObject.name);
    }
}
