using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Buffers.Text;
using System;

public class PlayerStats : MonoBehaviour
{
    #region Stats
    public int currLvl;
    public int currXp;

    [Header("Health")]
    public int currHP;
    public int currShield;

    private int baseMaxHP;
    public float percHPBuff;
    public int flatHPBuff;
    private int currMaxHP; // BaseHP * (1 + percHPBuff) + flatHPBuff

    [Header("Attack")]
    public float percATKBuff;
    public int flatATKBuff;
    private int baseATK;
    private int currATK; // BaseATK * (1 + percATKBuff) + flatATKBuff

    [Header("Defence")]
    public float percDEFBuff;
    public int flatDEFBuff;
    private int baseDEF;
    private int currDEF; // BaseDEF * (1 + percDEFBuff) + flatDEFBuff

    [Header("Stamina")]
    public float currStamina;
    public float currMaxStamina;
    #endregion

    public bool isInvulnerable;

    [SerializeField]
    private Slider HealthBar;
    [SerializeField]
    private Slider ExtraHealthBar;

    [SerializeField]
    private TextMeshProUGUI HealthText;

    private void Start()
    {
        SetBaseStatsFromLevel(currLvl);

        GameManager.Instance.equipmentMan.UpdatePlayerStatsUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SavePlayer();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadPlayer();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SetBaseStatsFromLevel(currLvl);

            GameManager.Instance.equipmentMan.UpdatePlayerStatsUI();
        }
    }

    public void SetHealth(int health)
    {
        if (health > currMaxHP)
        {
            currHP = currMaxHP;
        }
        else
        {
            currHP = health;
        }        

        HealthBar.value = currHP;
        

        HealthText.text = currHP + "/" + currMaxHP;
    }

    public void SetShield(int shield)
    {
        if (shield > currMaxHP)
        {
            currShield = currMaxHP;
        }
        else
        {
            currShield = shield;
        }

        ExtraHealthBar.value = currShield;
    }

    public void SetMaxHelath(int maxHeath, bool scaleCurrHealthPercentage)
    {
        if (scaleCurrHealthPercentage)
        {
            float perc = (float)currHP / currMaxHP;

            currHP = (int)(maxHeath * perc);
        }

        currMaxHP = maxHeath;
        HealthBar.maxValue = maxHeath;
        ExtraHealthBar.maxValue = maxHeath;

        HealthText.text = currHP + "/" + currMaxHP;
    }

    public void GetDamage(int damage)
    {
        // Calculate actual damage
        int dmg = damage;

        // Anti one shot: if health is more than 90% health is set to 1% 
        if (currHP >= (currMaxHP * 0.9f) && dmg >= (currShield + currHP))
        {
            currShield = 0;
            ExtraHealthBar.value = 0;

            currHP = (int)(currMaxHP * 0.01);
            HealthBar.value = currHP;

            return;
        }

        if (currShield != 0)
        {
            int newExtraHealth = currShield - dmg;

            if (newExtraHealth < 0)
            {
                currShield = 0;

                currHP += newExtraHealth;
            }
            else
            {
                currShield = newExtraHealth;
            }
        }
        else
        {
            if (dmg >= currHP)
            {
                currHP = 0;

                Die();
            }
            else
            {
                currHP -= dmg;
            }
        }

        HealthBar.value = currHP;
        ExtraHealthBar.value = currShield;

        HealthText.text = currHP + "/" + currMaxHP;
    }

    public void RegenHealth(int heal, bool isExtraHealth)
    {
        if (isExtraHealth)
        {
            if (heal >= currMaxHP - currShield)
            {
                currShield = currMaxHP;
            }
            else
            {
                currShield += heal;
            }
        }
        else
        {
            if (heal >= currMaxHP - currHP)
            {
                currHP = currMaxHP;
            }
            else
            {
                currHP += heal;
            }
        }

        HealthBar.value = currHP;
        ExtraHealthBar.value = currShield;

        HealthText.text = currHP + "/" + currMaxHP;
    }

    public void Die()
    {
        // Game Over screen
        // Restart form last save point
    }

    public void SavePlayer()
    {
        SaveAndLoadManager.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveAndLoadManager.loadPlayer();

        currLvl = data.currLvl;
        currXp = data.currXp;

        currHP = data.currHP;
        currShield = data.currShield;
        currMaxHP = data.currMaxHP;

        currATK = data.currATK;

        currDEF = data.currDEF;

        currStamina = data.currStamina;
        currMaxStamina = data.currMaxStamina;
    }

    public void UpdateCurrMaxHP()
    {
        currMaxHP = (int)(baseMaxHP * (1 + percHPBuff) + flatHPBuff);
    }

    public void UpdateCurrATK()
    {
        currATK = (int)(baseATK * (1 + percATKBuff) + flatATKBuff);
    }

    public void UpdateCurrDEF()
    {
        currDEF = (int)(baseDEF * (1 + percDEFBuff) + flatDEFBuff);
    }

    public void UpdateAllCurrStats()
    {
        UpdateCurrMaxHP();
        UpdateCurrATK();
        UpdateCurrDEF();
    }

    public int GetCurrMaxHP()
    {
        return currMaxHP;
    }

    public int GetCurrATK()
    {
        return currATK;
    }

    public int GetCurrDEF()
    {
        return currDEF;
    }

    public int XpForNextLevel(int level)
    {
        return (int)(0.01f * MathF.Pow(level, 2) + 2 * level + 100);
    }

    public void SetBaseStatsFromLevel(int level)
    {
        baseMaxHP = 10 * level + (int)Math.Log(level) * level;
        baseATK = 5 * level + (int)Math.Log(100 * level);
        baseDEF = 2 * level;
    }
}
