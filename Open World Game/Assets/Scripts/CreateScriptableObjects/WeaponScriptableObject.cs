using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Bow,
    Polearm,
    Claymore,
    Staff
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public string subStatType;
    public bool isSubStatPercentage;
    public float subStat;
    public int rarity;

    public Sprite icon;

    public string effectName;
    [TextArea(3, 10)]
    public string effect;
    [TextArea(3, 10)]
    public string description;

    //These ATK values are for the weapon ascension
    public float BaseATK1;
    public float BaseATK20;
    public float BaseATK40;
    public float BaseATK60;
    public float BaseATK70;
    public float BaseATK80;
    public float BaseATK90;
    public float BaseATK100;

    public float incrementATK;

    public float incrementSubStat; //Substat increment every five levels
                                   //Substat doesnt change on ascension

    public GameObject equipPrefab;

    //Create a script with all weapon effects. Each weapon will get the function of the effect from there.

    public virtual void Effect()
    {

    }
}
