using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfo : Interactable
{
    public WeaponScriptableObject scrObj;

    public string UniqueID;

    public bool hasDetailIcon; // Icon in the top left

    #region InventoryInfos
    //Top

    public float baseATK;
    public float currentSubstat;

    // Middle

    public int currentXp;
    public int xpForNextLevel;
    public int currentLevel;
    public int currentMaxLevel;
    public int ascensionLevel; // Remove
    public int refinementLevel; // Remove
    #endregion

    public override void Interact()
    {
        GameManager.Instance.invMan.AddItemToInventory(ItemType.Weapon, gameObject, 1);

        GameManager.Instance.plInteractMan.InRangeInteractables.Remove(gameObject);

        Destroy(gameObject);
    }

    public void Start()
    {
        baseATK = SetAtkFromLevel(currentLevel);
        xpForNextLevel = XpForNextLevel(currentLevel);
        currentSubstat = SetSecondaryStatFromLevel(currentLevel);
    }

    public float SetAtkFromLevel(int level)
    {
        float atk;

        if (level <= 20 && ascensionLevel == 0)
        {
            atk = scrObj.BaseATKs[0] + scrObj.incrementATK * (level - 1);
        }
        else if (level >= 20 && level <= 40 && ascensionLevel == 1)
        {
            atk = scrObj.BaseATKs[1] + scrObj.incrementATK * (level - 20);
        }
        else if (level >= 40 && level <= 60 && ascensionLevel == 2)
        {
            atk = scrObj.BaseATKs[2] + scrObj.incrementATK * (level - 40);
        }
        else if (level >= 60 && level <= 70 && ascensionLevel == 3)
        {
            atk = scrObj.BaseATKs[3] + scrObj.incrementATK * (level - 60);
        }
        else if (level >= 70 && level <= 80 && ascensionLevel == 4)
        {
            atk = scrObj.BaseATKs[4] + scrObj.incrementATK * (level - 70);
        }
        else if (level >= 80 && level <= 90 && ascensionLevel == 5)
        {
            atk = scrObj.BaseATKs[5] + scrObj.incrementATK * (level - 80);
        }
        else if (level >= 90 && level < 100 && ascensionLevel == 6)
        {
            atk = scrObj.BaseATKs[6] + scrObj.incrementATK * (level - 90);
        }
        else if (level == 100 && ascensionLevel == 7)
        {
            atk = scrObj.BaseATKs[7];
        }
        else
        {
            atk = 0;
            Debug.Log("Error: level out of range. Attack set to 0.");
        }

        return atk;
    }

    public float SetSecondaryStatFromLevel(int level)
    {
        float substat = (level / 5) * scrObj.incrementSubStat + scrObj.subStat;

        return substat;
    }

    public int XpForNextLevel(int level) // Xp to get from a level to the next. Parameter level is the lower level
    {
        int xp = 0;
        //level++;

        //if (scrObj.rarity == 1)
        //{
        //    xp = Mathf.RoundToInt(6 * Mathf.Pow(level, 2)) + 200;
        //}
        //else if (scrObj.rarity == 2)
        //{
        //    xp = Mathf.RoundToInt(10 * Mathf.Pow(level, 2)) + 300;
        //}
        //else if (scrObj.rarity == 3)
        //{
        //    if (level < 80)
        //    {
        //        xp = Mathf.RoundToInt(15 * Mathf.Pow(level, 2)) + 400;
        //    }
        //    else // level >= 80
        //    {
        //        xp = Mathf.RoundToInt(Mathf.Pow(level, 3)) + Mathf.RoundToInt(50 * Mathf.Pow(level, 2)) - 14000 * level - 384400;
        //    }
        //}
        //else if (scrObj.rarity == 4)
        //{
        //    if (level < 80)
        //    {
        //        xp = Mathf.RoundToInt(22 * Mathf.Pow(level, 2)) + 500;
        //    }
        //    else // level >= 80
        //    {
        //        xp = Mathf.RoundToInt(Mathf.Pow(level, 3)) + Mathf.RoundToInt(150 * Mathf.Pow(level, 2)) - 16620 * level - 1100;
        //    }
        //}
        //else if (scrObj.rarity == 5)
        //{
        //    if (level < 80)
        //    {
        //        xp = Mathf.RoundToInt(34 * Mathf.Pow(level, 2)) + 600;
        //    }
        //    else // level >= 80
        //    {
        //        xp = Mathf.RoundToInt(2 * Mathf.Pow(level, 3)) - 3900 * level - 499400;
        //    }
        //}
        //else
        //{
        //    xp = 0;
        //    Debug.Log("Error: Rarity int out of range. Xp for next level set to 0.");
        //}

        return xp;
    }

    public int XpFromAToB(int a, int b) // Total xp to get from level a to b
    {
        int totXp = 0;

        for (int i = a; i < b; i++)
        {
            totXp += XpForNextLevel(i);
        }

        return totXp;
    }

    public int NextMaxLevel(int level)
    {
        int nextLevel;

        if (level == 20)
        {
            nextLevel = 40;
        }
        else if (level == 40)
        {
            nextLevel = 60;
        }
        else if (level == 60)
        {
            nextLevel = 70;
        }
        else if (level == 70)
        {
            nextLevel = 80;
        }
        else if (level == 80)
        {
            nextLevel = 90;
        }
        else if (level == 90)
        {
            nextLevel = 100;
        }
        else
        {
            nextLevel = 0;
            Debug.Log("Error: trying to get unknown next max level");
        }

        return nextLevel;
    }

    public string StringWeaponType()
    {
        string type;

        if (scrObj.weaponType == WeaponType.Sword)
        {
            type = "Sword";
        }
        else if (scrObj.weaponType == WeaponType.Bow)
        {
            type = "Bow";
        }
        else if (scrObj.weaponType == WeaponType.Claymore)
        {
            type = "Claymore";
        }
        else if (scrObj.weaponType == WeaponType.Staff)
        {
            type = "Staff";
        }
        else if (scrObj.weaponType == WeaponType.Polearm)
        {
            type = "Polearm";
        }
        else
        {
            type = "Error";
        }

        return type;
    }

    public string GetIdentificationString()
    {
        string ID;

        ID = scrObj.ItemID.ToString() + "_" + currentLevel.ToString() + "_" + currentMaxLevel.ToString() + "_" + ascensionLevel.ToString() + "_" + currentXp.ToString();

        return ID;
    }

    public void EquipWeapon()
    {
        GameManager.Instance.plStats.currWeaponAttack = baseATK;

        // Set weapon GFX on player
    }

    public void DropWeapon()
    {

    }
}
