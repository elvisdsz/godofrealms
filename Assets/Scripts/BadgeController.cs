using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeController : MonoBehaviour
{
    public Image fillImage;
    public Image bgImage;

    public void Init(Sprite realmIcon, Color color, float realmControl)
    {
        bgImage.sprite = realmIcon;
        fillImage.sprite = realmIcon;
        fillImage.color = color;
        UpdateRealmControlValue(realmControl);
    }

    public void UpdateRealmControlValue(float realmControl)
    {
        fillImage.fillAmount = realmControl;
    }
}
