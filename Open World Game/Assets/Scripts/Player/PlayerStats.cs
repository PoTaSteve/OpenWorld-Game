using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    #region Stats
    public int currLvl;
    public int currXp;

    public int currHealth;
    public int currExtraHealth;
    public int currMaxHealth;
    
    public float currAtk;

    public float currDef;

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
    }

    public void SetHealth(int health, int extraHealth)
    {
        if (health > currMaxHealth)
        {
            currHealth = currMaxHealth;
        }
        else
        {
            currHealth = health;
        }

        if (extraHealth > currMaxHealth)
        {
            currExtraHealth = currMaxHealth;
        }
        else
        {
            currExtraHealth = extraHealth;
        }

        HealthBar.value = currHealth;
        ExtraHealthBar.value = currExtraHealth;

        HealthText.text = currHealth + "/" + currMaxHealth;
    }

    public void SetMaxHelath(int maxHeath)
    {
        currMaxHealth = maxHeath;
        HealthBar.maxValue = maxHeath;
        ExtraHealthBar.maxValue = maxHeath;

        HealthText.text = currHealth + "/" + currMaxHealth;
    }

    public void GetDamage(int damage)
    {
        // Calculate actual damage
        int dmg = damage;

        // Anti one shot: if health is more than 90% health is set to 1% 
        if (currHealth >= (currMaxHealth * 0.9f) && dmg >= (currExtraHealth + currHealth))
        {
            currExtraHealth = 0;
            ExtraHealthBar.value = 0;

            currHealth = (int)(currMaxHealth * 0.01);
            HealthBar.value = currHealth;

            return;
        }

        if (currExtraHealth != 0)
        {
            int newExtraHealth = currExtraHealth - dmg;

            if (newExtraHealth < 0)
            {
                currExtraHealth = 0;

                currHealth += newExtraHealth;
            }
            else
            {
                currExtraHealth = newExtraHealth;
            }
        }
        else
        {
            if (dmg >= currHealth)
            {
                currHealth = 0;

                Die();
            }
            else
            {
                currHealth -= dmg;
            }
        }

        HealthBar.value = currHealth;
        ExtraHealthBar.value = currExtraHealth;

        HealthText.text = currHealth + "/" + currMaxHealth;
    }

    public void RegenHealth(int heal, bool isExtraHealth)
    {
        if (isExtraHealth)
        {
            if (heal >= currMaxHealth - currExtraHealth)
            {
                currExtraHealth = currMaxHealth;
            }
            else
            {
                currExtraHealth += heal;
            }
        }
        else
        {
            if (heal >= currMaxHealth - currHealth)
            {
                currHealth = currMaxHealth;
            }
            else
            {
                currHealth += heal;
            }
        }

        HealthBar.value = currHealth;
        ExtraHealthBar.value = currExtraHealth;

        HealthText.text = currHealth + "/" + currMaxHealth;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();

            GetDamage(enemy.damage);
        }
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

        currHealth = data.currHealth;
        currExtraHealth = data.currExtraHealth;
        currMaxHealth = data.currMaxHealth;

        currAtk = data.currAtk;

        currDef = data.currDef;

        currStamina = data.currStamina;
        currMaxStamina = data.currMaxStamina;
    }
}
