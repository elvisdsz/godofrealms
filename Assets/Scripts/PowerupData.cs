using System.Collections;
using System.Collections.Generic;

public class PowerupData
{
    public enum PowerupType {BUILD_BRIDGE, SPEED_UP, ATTRACT_SOUL, ETERNAL_RELEASE};
    
    private Dictionary<PowerupType, bool> powerup = new Dictionary<PowerupType, bool>();

    public PowerupData()
    {
        powerup.Add(PowerupType.BUILD_BRIDGE, false);
        powerup.Add(PowerupType.SPEED_UP, false);
        powerup.Add(PowerupType.ATTRACT_SOUL, false);
        powerup.Add(PowerupType.ETERNAL_RELEASE, false);
    }

    public void EnablePowerup(PowerupData.PowerupType powerupType)
    {
        powerup[powerupType] = true;
    }

    public bool IsPowerupEnabled(PowerupData.PowerupType powerupType)
    {
        return powerup[powerupType];
    }
}