using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    #region Stats
    public int currHealth;
    public int currMaxHealth;
    public int currSpecialHealth;

    public int currLvl;
    public int currXp;
    
    public float currAtk;

    public float currStamina;
    public float currMaxStamina;
    #endregion

    [SerializeField]
    private Transform HealthBar;

    public void SetHealth()
    {

    }
}
