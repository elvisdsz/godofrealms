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

    IEnumerator Blast(Vector3Int position)
    {
        blastOn = true;

        Vector3Int[] pattern1 = {position, position+Vector3Int.up, position+Vector3Int.down, position+Vector3Int.right, position+Vector3Int.left,
            position+Vector3Int.up+Vector3Int.right, position+Vector3Int.up+Vector3Int.left,
            position+Vector3Int.down+Vector3Int.right, position+Vector3Int.down+Vector3Int.left,
        };

        Vector3Int[] pattern2 = {position, position+Vector3Int.up, position+Vector3Int.down, position+Vector3Int.right, position+Vector3Int.left,
            position+Vector3Int.up+Vector3Int.up+Vector3Int.right+Vector3Int.right, position+Vector3Int.up+Vector3Int.up+Vector3Int.left+Vector3Int.left,
            position+Vector3Int.down+Vector3Int.down+Vector3Int.right+Vector3Int.right, position+Vector3Int.down+Vector3Int.down+Vector3Int.left+Vector3Int.left,

            position+Vector3Int.up+Vector3Int.up+Vector3Int.up, position+Vector3Int.left+Vector3Int.left+Vector3Int.left,
            position+Vector3Int.down+Vector3Int.down+Vector3Int.down, position+Vector3Int.right+Vector3Int.right+Vector3Int.right,
        };

        Vector3Int[] pattern3 = {
            position+Vector3Int.up+Vector3Int.up+Vector3Int.up+Vector3Int.up, position+Vector3Int.left+Vector3Int.left+Vector3Int.left+Vector3Int.left,
            position+Vector3Int.down+Vector3Int.down+Vector3Int.down+Vector3Int.down, position+Vector3Int.right+Vector3Int.right+Vector3Int.right+Vector3Int.right,

            position+Vector3Int.up+Vector3Int.up+Vector3Int.up+Vector3Int.right+Vector3Int.right+Vector3Int.right, position+Vector3Int.up+Vector3Int.up+Vector3Int.up+Vector3Int.left+Vector3Int.left+Vector3Int.left,
            position+Vector3Int.down+Vector3Int.down+Vector3Int.down+Vector3Int.right+Vector3Int.right+Vector3Int.right, position+Vector3Int.down+Vector3Int.down+Vector3Int.down+Vector3Int.left+Vector3Int.left+Vector3Int.left,
        };

        ChangeTileColor(tilemap, position, pattern1, this.realmColor);
        yield return new WaitForSeconds(0.15f);
        ChangeTileColor(tilemap, position, pattern1, Color.white);
        ChangeTileColor(tilemap, position, pattern2, this.realmColor);
        yield return new WaitForSeconds(0.2f);
        ChangeTileColor(tilemap, position, pattern2, Color.white);
        ChangeTileColor(tilemap, position, pattern3, this.realmColor);
        yield return new WaitForSeconds(0.2f);
        ChangeTileColor(tilemap, position, pattern3, Color.white);
        blastOn = false;
    }

    private void ChangeTileColor(Tilemap tilemap, Vector3Int position, Vector3Int[] pattern, Color color) {
        /*Vector3Int[] positionArr2 = {position, position+Vector3Int.up, position+Vector3Int.down, position+Vector3Int.right, position+Vector3Int.left,
            position+Vector3Int.up+Vector3Int.up+Vector3Int.right+Vector3Int.right, position+Vector3Int.up+Vector3Int.up+Vector3Int.left+Vector3Int.left,
            position+Vector3Int.down+Vector3Int.down+Vector3Int.right+Vector3Int.right, position+Vector3Int.down+Vector3Int.down+Vector3Int.left+Vector3Int.left,
        };*/

        for(int i=0; i<pattern.Length; i++) {
            Vector3Int ipos = pattern[i];
            if(tilemap.HasTile(position)) {
                tilemap.SetTileFlags(ipos, TileFlags.None);
                tilemap.SetColor(ipos, color);
            }
        }
    }

}
