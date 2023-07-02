using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class EnemyScrObj : ScriptableObject
{
    public int attack;
    public int defense;
    public int speed;
    public int heath;
}
