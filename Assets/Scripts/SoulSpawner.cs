using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    public GameObject soulPrefab;
    public Transform[] spawnPoints;

    private RealmManager realmManager;

    private float spawnTimeInterval=5f;
    private float timeSinceLastSpawn=0f;

    // Start is called before the first frame update
    void Start()
    {
        realmManager = GetComponent<RealmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnPoints.Length>0 && realmManager.GetSoulFraction()<1)
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
