using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealmManager : MonoBehaviour
{
    private GameManagerScript gameManager;
    public Color realmColor;
    public bool acquired = false;
    public int maxSoulCount = 10;
    public int maxSoulThisWave = 0;
    public int maxChaosWaves = 2;
    public int chaosWavesCompleted = 0;
    public bool chaosWaveOn = false;

    public enum Realm
    {
        FIRE,
        WATER,
        GATE
    }
    public Realm currentRealm;
    [SerializeField] private int releasedSoulCount = 0;

    private Tilemap tilemap;

    private List<SoulController> soulList = new List<SoulController>();
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManagerScript._instance;
        tilemap = GetComponent<Tilemap>();
        maxSoulThisWave = maxSoulCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Hit(Vector3 origin)
    {
        GridLayout grid = GetComponentInParent<GridLayout>();
        Vector3Int gridPosition = grid.WorldToCell(origin);

        // Debug.Log("Blast at "+ gridPosition);
        StartCoroutine(Blast(gridPosition));
            
        tilemap.color = Color.Lerp(Color.white, realmColor, GetRealmControlFraction());
    }

    public float GetReleasedSoulFraction()
    {
        return (float)releasedSoulCount/maxSoulThisWave;
    }

    public float GetSoulFraction()
    {
        return (float)soulList.Count/maxSoulThisWave;
    }

    public float GetRealmControlFraction()
    {
        if(acquired)
            return 1-(float)soulList.Count/maxSoulCount;
        else
            return GetReleasedSoulFraction();
    }

    public int GetSoulCount()
    {
        return soulList.Count;
    }

    public bool SpawnMoreSouls()
    {
        return (soulList.Count+releasedSoulCount)<maxSoulThisWave;
    }

    public bool AddSoulToRealm(SoulController soul)
    {
        if(soulList.Count >= maxSoulThisWave)
            return false;
            
        soulList.Add(soul);
        tilemap.color = Color.Lerp(Color.white, realmColor, GetRealmControlFraction());
        return true;
    }

    public bool RemoveSoulFromRealm(SoulController soul, Vector3 origin)
    {
        if(!soulList.Contains(soul))
            return false;

        releasedSoulCount += 1;
        gameManager.SoulReleased();
        if(releasedSoulCount == maxSoulThisWave && soulList.Count == 1) {
            EndChaosWave();
        }
        GameObject.Destroy(soul.gameObject);
        bool success = soulList.Remove(soul);
        Hit(origin);
        return success;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            gameManager.SetPlayerRealm(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            gameManager.ResetPlayerRealm(this);
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

    public void TriggerChaosWave(float difficultyNormalized)
    {
        if(chaosWaveOn || soulList.Count!=0 || chaosWavesCompleted==maxChaosWaves)
            return;
        
        chaosWaveOn = true;
        maxSoulThisWave = (int)Mathf.Ceil(maxSoulCount * difficultyNormalized);
        maxSoulThisWave = maxSoulThisWave==0? 1: maxSoulThisWave;
        releasedSoulCount = 0;
    }

    private void EndChaosWave()
    {
        chaosWaveOn = false;
        chaosWavesCompleted += 1;
        acquired = true;
        gameManager.ChaosWaveEnded(this);
        Debug.Log("Realm acquired");
    }

}
