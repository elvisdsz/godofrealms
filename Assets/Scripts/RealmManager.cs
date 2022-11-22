using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RealmManager : MonoBehaviour
{
    private GameManagerScript gameManager;
    public Color realmColor;
    public Sprite realmIcon;
    public bool acquired = false;
    public int maxSoulCount = 10;
    public int maxSoulThisWave = 0;
    public bool chaosPossible = true;
    public int chaosWavesCompleted = 0;
    public bool chaosWaveOn = false;

    public enum Realm{ TUTORIAL, FIRE, WATER, EARTH, METAL, FINAL, GATE }

    public Realm currentRealm;
    public PowerupData.PowerupType currentPowerUpType;
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
        
        if(!acquired)
            tilemap.color = Color.Lerp(Color.white, realmColor, GetRealmControlFraction());
        else
            tilemap.color = realmColor;
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
        if(!acquired)
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

    public Vector3Int GetPositionOnTilemap(Vector2 position)
    {
        GridLayout grid = GetComponentInParent<GridLayout>();
        Vector3Int gridPosition = grid.WorldToCell(position);

        return gridPosition;
    }

    public IEnumerator Blast(Vector3Int position)
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
        AudioManager.instance.Play("SoulRelease");
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

    public bool TriggerChaosWave(float difficultyNormalized)
    {
        if(!chaosPossible || chaosWaveOn || soulList.Count!=0)
            return false;
        
        chaosWaveOn = true;
        maxSoulThisWave = (int)Mathf.Ceil(maxSoulCount * difficultyNormalized);
        maxSoulThisWave = maxSoulThisWave==0? 1: maxSoulThisWave;
        releasedSoulCount = 0;
        return true;
    }

    private void EndChaosWave()
    {
        chaosWaveOn = false;
        chaosWavesCompleted += 1;
        acquired = true;
        tilemap.color = Color.Lerp(Color.white, realmColor, GetRealmControlFraction());
        gameManager.ChaosWaveEnded(this);
        Debug.Log("Realm acquired");
    }

}
