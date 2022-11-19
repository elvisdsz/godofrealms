using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript _instance;

    PlayerMovement player;
    private int totalSoulsReleased = 0;
    private float soulReleaseAverageTime = 0f;
    private float timeSinceLastSoulRelease = 0f;


    void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // if player in realm and realm has at least one soul
        if(player.GetRealm() != null && player.GetRealm().GetSoulCount()>0)
            timeSinceLastSoulRelease += Time.deltaTime;
    }

    public void SoulReleased()
    {
        // Calculate new average soul release time
        float totalRealmPlaytime = (soulReleaseAverageTime*totalSoulsReleased)+timeSinceLastSoulRelease;
        totalSoulsReleased += 1;
        soulReleaseAverageTime = totalRealmPlaytime/totalSoulsReleased; 
        timeSinceLastSoulRelease = 0f;
    }

    public float GetSoulReleaseTime()
    {
        return soulReleaseAverageTime;
    }

    public int GetTotalSoulsReleased()
    {
        return totalSoulsReleased;
    }
}
