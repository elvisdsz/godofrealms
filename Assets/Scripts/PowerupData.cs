using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupData
{
    public enum PowerupType {BUILD_BRIDGE, SPEED_UP, ATTRACT_SOUL, NONE};
    
    private Dictionary<PowerupType, float> powerup = new Dictionary<PowerupType, float>();

    public PowerupData()
    {
        powerup.Add(PowerupType.BUILD_BRIDGE, 0f);
        powerup.Add(PowerupType.SPEED_UP, 0f);
        powerup.Add(PowerupType.ATTRACT_SOUL, 0f);
        powerup.Add(PowerupType.NONE, 0f);
    }

    public void EnablePowerup(PowerupData.PowerupType powerupType)
    {
        powerup[powerupType] = 1f;
    }

    public void UpdatePowerup(PowerupData.PowerupType powerupType, float newValue)
    {
        powerup[powerupType] = newValue;
    }

    public float PowerupValue(PowerupData.PowerupType powerupType)
    {
        return powerup[powerupType];
    }
}