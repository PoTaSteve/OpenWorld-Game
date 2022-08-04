using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currLvl;
    public int currXp;

    public int currHealth;
    public int currExtraHealth;
    public int currMaxHealth;

    public float currAtk;

    public float currDef;

    public float currStamina;
    public float currMaxStamina;

    public PlayerData (PlayerStats player)
    {
        currLvl = player.currLvl;
        currXp = player.currXp;

        currHealth = player.currHealth;
        currExtraHealth = player.currExtraHealth;
        currMaxHealth = player.currMaxHealth;

        currAtk = player.currAtk;

        currDef = player.currDef;

        currStamina = player.currStamina;
        currMaxStamina = player.currMaxStamina;
    }
}
