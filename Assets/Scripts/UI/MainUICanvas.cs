using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUICanvas : MonoBehaviour
{
    public static MainUICanvas _instance;
    public GameObject startScreenCanvas;
    public GameObject pauseScreenCanvas;
    public GameObject winScreenPanel;
    public GameObject bridgeFailPanel;
    public GameObject firstChaosPanel;
    public TextMeshProUGUI firstChaosText;

    void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
			//DontDestroyOnLoad(gameObject);
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        // startScreenCanvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale==1) {
            PauseGame();
        }
    }

    public void ShowStartScreenCanvas()
    {
        startScreenCanvas.SetActive(true);
    }

    public void StartGame()
    {
        startScreenCanvas.SetActive(false);
        GameManagerScript._instance.StartGame();
    }

    public void PauseGame()
    {
        pauseScreenCanvas.SetActive(true);
        GameManagerScript._instance.PauseGame();
    }

    public void ResumeGame()
    {
        HideAllPanels();
        pauseScreenCanvas.SetActive(false);
        GameManagerScript._instance.ResumeGame();
    }

    public void QuitGame()
    {
        pauseScreenCanvas.SetActive(false);
        GameManagerScript._instance.QuitGame();
    }

    public void RestartGame()
    {
        pauseScreenCanvas.SetActive(false);
        GameManagerScript._instance.RestartGame();
    }

    public void HideAllPanels()
    {
        startScreenCanvas.SetActive(false);
        pauseScreenCanvas.SetActive(false);
        winScreenPanel.SetActive(false);
        bridgeFailPanel.SetActive(false);
        firstChaosPanel.SetActive(false);
    }

    public void ShowBridgeFailPanel()
    {
        HideAllPanels();
        bridgeFailPanel.SetActive(true);
    }

    public void ShowFirstChaosPanel(RealmManager realm)
    {
        HideAllPanels();
        firstChaosText.text = "There is some chaos in the "+realm.currentRealm.ToString()+" realm. Some wandering souls are back. If you lose control of the realm, it might affect your ability to "+realm.currentPowerUpType.ToString();
        if(realm.currentPowerUpType != PowerupData.PowerupType.NONE)
            firstChaosPanel.SetActive(true);
    }

    public void ShowWinScreenPanel()
    {
        HideAllPanels();
        winScreenPanel.SetActive(true);
    }
}
