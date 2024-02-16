using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class EnemyScrObj : ScriptableObject
{
    public string enemyID;

    public string enemyName;

    public int baseAttack;
    public int baseDefense;
    public int baseSpeed;
    public int maxHealth;
}
