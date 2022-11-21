using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript _instance;

    public TMP_FontAsset typerFont;

    private List<RealmManager> visitedRealms = new List<RealmManager>();

    private PlayerMovement player;
    private GameIndicators gameIndicators;
    private MainUICanvas mainUICanvas;
    private int totalSoulsReleased = 0;
    private float soulReleaseAverageTime = 0f;
    private float lastReleaseAverageTime = 4f;
    private float timeSinceLastSoulRelease = 0f;
    private int goldGateNum = 0;
    private bool winGame = false;

    [SerializeField] private float chaosTriggerCountdownTimer=0f;


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
        gameIndicators = GameIndicators._instance;
        mainUICanvas = MainUICanvas._instance;

        gameIndicators.HideHUD();
        mainUICanvas.ShowStartScreenCanvas();
        PauseGame();
    }

    public void StartGame()
    {
        ResumeGame();
        gameIndicators.ShowHUD();
    }

    public void PauseGame()
    {
        gameIndicators.HideHUD();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        gameIndicators.ShowHUD();
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("WorldScene");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if(chaosTriggerCountdownTimer > 0f)
        {
            chaosTriggerCountdownTimer -= Time.deltaTime;
            if(chaosTriggerCountdownTimer <= 0f)
            {
                chaosTriggerCountdownTimer = 0f;
                TriggerChaos();
            }
        }

        // If no chaos anywhere, create chaos
        if(visitedRealms.Count>1 && GetAllAcquiredNonChaoticRealms().Count == 0f)
            TriggerChaos();

        if(player.GetRealm() != null)
        {
            gameIndicators.UpdateRealmControlMeter(player.GetRealm().GetRealmControlFraction());

            // if player in realm and realm has at least one soul
            if(player.GetRealm().GetSoulCount()>0)
                timeSinceLastSoulRelease += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.RightControl))
            {
                player.GetRealm().TriggerChaosWave(0.5f);
            }
        }

        UpdateRealmIndicators();
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

    public void SetPlayerRealm(RealmManager realm)
    {
        player.SetNewRealm(realm);
        
        if(realm.currentRealm == RealmManager.Realm.TUTORIAL)
            return;

        if(!visitedRealms.Contains(realm))
        {
            visitedRealms.Add(realm);
            HandleNewRealm(realm);
        }

        gameIndicators.ShowRealmControlMeter(realm.realmColor, realm.GetReleasedSoulFraction());
        if(realm.chaosWavesCompleted <= 0)
            TriggerChaos(realm, 1f);

        // Calc gold gates and see if wins
        if(goldGateNum >= 3 && realm.currentRealm == RealmManager.Realm.FINAL)
        {
            winGame = true;
            Debug.Log("WIN "+winGame);
        }
    }

    public void ResetPlayerRealm(RealmManager realm)
    {
        player.ResetRealm(realm);
        gameIndicators.HideRealmControlMeter();
    }

    private void TriggerChaos()
    {
        // choose a random acquired non-chaotic realm
        RealmManager realm = ChooseRandomRealmForChaos();
        
        TriggerChaos(realm, (soulReleaseAverageTime/lastReleaseAverageTime)/2);
    }

    private void TriggerChaos(RealmManager realm, float difficultyNormalized)
    {
        if(realm == null)
        {
            if(player.GetRealm() != null)
                chaosTriggerCountdownTimer = player.GetRealm().GetSoulCount() * soulReleaseAverageTime;
            chaosTriggerCountdownTimer = chaosTriggerCountdownTimer<=1f? 5f: chaosTriggerCountdownTimer;
            return;
        }

        Debug.Log("Attempting to trigger chaos in realm "+realm.name+" with normalized difficulty ="+difficultyNormalized);

        // trigger chaos in chosen realm
        realm.TriggerChaosWave(difficultyNormalized);

        // reset countdown timer with new soul count and player perf
        chaosTriggerCountdownTimer = realm.maxSoulThisWave * soulReleaseAverageTime;
    }

    public void ChaosWaveEnded(RealmManager realm)
    {
        lastReleaseAverageTime = soulReleaseAverageTime;
    }

    private RealmManager ChooseRandomRealmForChaos()
    {
        // Random acquired non-chaotic realm
        List<RealmManager> candidateRealmList = GetAllAcquiredNonChaoticRealms();

        if(candidateRealmList.Count == 0)
            return null;
        
        int randomIndex = Random.Range(0, candidateRealmList.Count);
        return candidateRealmList[randomIndex];
    }

    private List<RealmManager> GetAllAcquiredNonChaoticRealms()
    {
        List<RealmManager> realmList = new List<RealmManager>();
        foreach(RealmManager realm in visitedRealms)
            if(realm.chaosPossible && realm.acquired && !realm.chaosWaveOn)
                realmList.Add(realm);
        
        return realmList;
    }

    private void HandleNewRealm(RealmManager realm)
    {
        gameIndicators.AddNewRealmBadge(realm, realm.GetRealmControlFraction());
    }

    private void UpdateRealmIndicators()
    {
        foreach(RealmManager realm in visitedRealms)
        {
            gameIndicators.UpdateRealmBadge(realm, realm.GetRealmControlFraction());
            if(realm.acquired)
            {
                player.powerupData.UpdatePowerup(realm.currentPowerUpType, realm.GetRealmControlFraction());
            }
        }
    }

    public void OpenGoldGate()
    {
        goldGateNum += 1;
        Debug.Log("open gold gate");
    }
}
