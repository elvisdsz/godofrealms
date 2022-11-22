using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Subtitle : MonoBehaviour
{

    public TextMeshProUGUI subtitleTextObj;

    public static Subtitle _instance;

    void Awake()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowSubtitle(string str, float time)
    {
        StartCoroutine(DisplaySubtitleText(str, time));
    }

    IEnumerator DisplaySubtitleText(string str, float seconds)
    {
        subtitleTextObj.text = str;
        yield return new WaitForSeconds(seconds);
        if(subtitleTextObj.text == str)
            subtitleTextObj.text = "";
    }
}
