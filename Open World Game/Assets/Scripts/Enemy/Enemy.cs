using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public EnemyScrObj enemyScrObj;

    public GameEvent onEnemyKilled;


    public GameObject DamageText;
    public Transform damageParent;

    public float currHealth;
    public Slider healthbar;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealthBar(enemyScrObj.maxHealth);

        currHealth = enemyScrObj.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMaxHealthBar(int maxHealth)
    {
        healthbar.maxValue = maxHealth;
        healthbar.value = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage - enemyScrObj.baseDefense;

        SpawnDamageNumber((int)damage);

        UpdateHealthBar();

        if (currHealth <= 0)
        {
            Destroy(gameObject);

            // Call event for enemy's death
            onEnemyKilled.Invoke(enemyScrObj.enemyID, enemyScrObj.name);
        }
    }

    public void SpawnDamageNumber(int damage)
    {
        GameObject dmgText = Instantiate(DamageText, damageParent);
        dmgText.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    public void UpdateHealthBar()
    {
        healthbar.value = currHealth;
    }
}
