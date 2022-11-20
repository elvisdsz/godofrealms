using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameIndicators : MonoBehaviour
{
    public static GameIndicators _instance;

    public GameObject realmControlSliderGO;
    private Slider realmControlSlider;
    public Image meterFillImage;
    public GameObject BadgeBeltPanel;
    public GameObject BadgePrefab;
    private Dictionary<RealmManager, BadgeController> realmBadgeDict = new Dictionary<RealmManager, BadgeController>();

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
        realmControlSliderGO.SetActive(false);
        realmControlSlider = realmControlSliderGO.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRealmControlMeter(Color color, float value)
    {
        realmControlSliderGO.SetActive(true);
        meterFillImage.color = color;
        realmControlSlider.value = value;
    }

    public void UpdateRealmControlMeter(float value)
    {
        realmControlSlider.value = value;
    }

    public void HideRealmControlMeter()
    {
        realmControlSliderGO.SetActive(false);
    }

    public void AddNewRealmBadge(RealmManager realm, float realmControl)
    {
        GameObject newBadge = Instantiate(BadgePrefab);
        newBadge.transform.SetParent(BadgeBeltPanel.transform, false);
        BadgeController badgeController = newBadge.GetComponent<BadgeController>();
        badgeController.Init(realm.realmIcon, realm.realmColor, realmControl);
        realmBadgeDict.Add(realm, badgeController);
    }

    public void UpdateRealmBadge(RealmManager realm, float realmControl)
    {
        if(!realmBadgeDict.ContainsKey(realm))
            return;

        BadgeController badgeController = realmBadgeDict[realm];
        badgeController.UpdateRealmControlValue(realmControl);
    }
}
