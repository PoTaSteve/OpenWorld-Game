using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currLvl;
    public int currXp;

    public int currHP;
    public int currShield;
    public int currMaxHP;

    public int currATK;

    public int currDEF;

    public float currStamina;
    public float currMaxStamina;

    public PlayerData (PlayerStats player)
    {
        currLvl = player.currLvl;
        currXp = player.currXp;

        currHP = player.currHP;
        currShield = player.currShield;
        currMaxHP = player.GetCurrMaxHP();

        currATK = player.GetCurrATK();

        currDEF = player.GetCurrDEF();

        currStamina = player.currStamina;
        currMaxStamina = player.currMaxStamina;
    }
}
