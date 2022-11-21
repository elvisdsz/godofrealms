using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUICanvas : MonoBehaviour
{
    public static MainUICanvas _instance;
    public GameObject startScreenCanvas;

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
        startScreenCanvas.SetActive(false);
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
}
