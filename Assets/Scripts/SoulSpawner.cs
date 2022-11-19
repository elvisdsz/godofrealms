using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    private GameManagerScript gameManager;
    public GameObject soulPrefab;
    public Transform[] spawnPoints;

    private RealmManager realmManager;

    private float spawnTimeInterval=5f;
    private float timeSinceLastSpawn=0f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManagerScript._instance;
        realmManager = GetComponent<RealmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Chaos wave needs to be triggered via the realm manager
        if(!realmManager.chaosWaveOn)
            return;

        // Skip soul spawning if player hasn't demonstrated release soul mechanic
        if(spawnPoints.Length<0 || (gameManager.GetTotalSoulsReleased()<1 && realmManager.GetSoulCount()>2))
            return;

        if(realmManager.SpawnMoreSouls())
        {
            timeSinceLastSpawn += Time.deltaTime;
            if(timeSinceLastSpawn >= spawnTimeInterval || realmManager.GetSoulFraction()==0f) {
                string pickedWord = WordBank.PickWord(realmManager, 1);
                Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                SoulController soulController = SpawnSoul(spawnPosition, pickedWord);
                if(soulController != null)
                    timeSinceLastSpawn = 0f;
            }
        }
    }

    private SoulController SpawnSoul(Vector3 position, string word) {

        if(!realmManager.IsPositionOnTilemap(position)) {
            Debug.Log("Failed to spawn soul. Position ("+position+") is not within realm's bounds");
            return null;
        }

        GameObject soul = Instantiate(soulPrefab);
        soul.transform.position = position;
        SoulController soulController = soul.GetComponent<SoulController>();
        soulController.Init(word, realmManager);

        if(realmManager.AddSoulToRealm(soulController))
            return soulController;
        else {
            GameObject.Destroy(soul);
            return null;
        }
    }
}
