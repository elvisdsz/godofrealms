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
    private bool blastOn = false;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(Vector3 origin)
    {
        if(blastOn)    // test: remove once triggered per soul
            return;

        GridLayout grid = GetComponentInParent<GridLayout>();
        Vector3Int gridPosition = grid.WorldToCell(origin);

        // Debug.Log("Blast at "+ gridPosition);
        StartCoroutine(Blast(gridPosition));

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
            player.SetNewRealm(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            PlayerMovement player = collider.gameObject.GetComponent<PlayerMovement>();
            player.ResetRealm(this);
        }
    }

    public bool IsPositionOnTilemap(Vector2 position)
    {
        GridLayout grid = GetComponentInParent<GridLayout>();
        Vector3Int gridPosition = grid.WorldToCell(position);

        return tilemap.HasTile(gridPosition);
    }

    IEnumerator Blast(Vector3Int gridPosition)
    {
        blastOn = true;
        ChangeTileColor(tilemap, gridPosition, this.realmColor);
        yield return new WaitForSeconds(0.5f);
        ChangeTileColor(tilemap, gridPosition, Color.white);
        blastOn = false;
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

}
