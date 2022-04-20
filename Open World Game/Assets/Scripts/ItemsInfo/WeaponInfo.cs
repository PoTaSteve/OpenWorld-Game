using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfo : Interactable
{
    public WeaponScriptableObject scrObj;

    public string UniqueID;

    #region InventoryInfos
    //Top

    public float baseATK;
    public float currentSubstat;

    // Middle

    public int currentXp;
    public int xpForNextLevel;
    public int currentLevel;
    public int currentMaxLevel;
    public int ascensionLevel;
    public int refinementLevel;
    public bool isLocked;
    #endregion

    public override void Interact()
    {
        #region Set Inventory Slot

        // Instantiate Slot: WeaponInfo script
        WeaponInfo newSlot = Instantiate<WeaponInfo>(GameManager.Instance.invMan.WeaponInvSlot.GetComponent<WeaponInfo>(), GameManager.Instance.invMan.TabsContent[0]);
        GameManager.Instance.invMan.WeaponsTab.Add(GetIdentificationString());
        newSlot.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.invMan.UpdateWeaponInvSlotDetails(newSlot.gameObject); });

        newSlot.scrObj = scrObj;

        newSlot.baseATK = SetAtkFromLevel(currentLevel);
        newSlot.currentSubstat = SetSecondaryStatFromLevel(currentLevel);

        newSlot.currentXp = currentXp;
        xpForNextLevel = XpForNextLevel(currentLevel);
        newSlot.xpForNextLevel = xpForNextLevel;

        newSlot.currentLevel = currentLevel;
        newSlot.currentMaxLevel = currentMaxLevel;
        newSlot.ascensionLevel = ascensionLevel;
        newSlot.refinementLevel = refinementLevel;
        newSlot.isLocked = isLocked;

        newSlot.UniqueID = newSlot.GetIdentificationString();

        // Instantiate Slot: Slot UI
        newSlot.transform.GetChild(2).gameObject.SetActive(false);
        newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lv. " + newSlot.currentLevel;
        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = newSlot.scrObj.icon;

        Transform Rarity = newSlot.transform.GetChild(3);
        foreach (Transform t in Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < newSlot.scrObj.rarity; i++)
        {
            Rarity.GetChild(i).gameObject.SetActive(true);
        }

        if (newSlot.isLocked)
        {
            newSlot.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
            //newSlot.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Image>().color = GameManager.Instance.invMan.closedPadlockColBG;
            //newSlot.transform.GetChild(5).GetChild(0).GetChild(2).GetComponent<Image>().sprite = GameManager.Instance.invMan.PadlockClosed;
            //newSlot.transform.GetChild(5).GetChild(0).GetChild(2).GetComponent<Image>().color = GameManager.Instance.invMan.closedPadlockCol;
        }
        else
        {
            newSlot.transform.GetChild(5).GetChild(0).gameObject.SetActive(false);
        }

        newSlot.transform.GetChild(5).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = newSlot.refinementLevel.ToString();

        newSlot.transform.GetChild(6).gameObject.SetActive(true);

        #endregion

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
        int xp;
        level++;

        if (scrObj.rarity == 1)
        {
            xp = Mathf.RoundToInt(6 * Mathf.Pow(level, 2)) + 200;
        }
        else if (scrObj.rarity == 2)
        {
            xp = Mathf.RoundToInt(10 * Mathf.Pow(level, 2)) + 300;
        }
        else if (scrObj.rarity == 3)
        {
            if (level < 80)
            {
                xp = Mathf.RoundToInt(15 * Mathf.Pow(level, 2)) + 400;
            }
            else // level >= 80
            {
                xp = Mathf.RoundToInt(Mathf.Pow(level, 3)) + Mathf.RoundToInt(50 * Mathf.Pow(level, 2)) - 14000 * level - 384400;
            }
        }
        else if (scrObj.rarity == 4)
        {
            if (level < 80)
            {
                xp = Mathf.RoundToInt(22 * Mathf.Pow(level, 2)) + 500;
            }
            else // level >= 80
            {
                xp = Mathf.RoundToInt(Mathf.Pow(level, 3)) + Mathf.RoundToInt(150 * Mathf.Pow(level, 2)) - 16620 * level - 1100;
            }
        }
        else if (scrObj.rarity == 5)
        {
            if (level < 80)
            {
                xp = Mathf.RoundToInt(34 * Mathf.Pow(level, 2)) + 600;
            }
            else // level >= 80
            {
                xp = Mathf.RoundToInt(2 * Mathf.Pow(level, 3)) - 3900 * level - 499400;
            }
        }
        else
        {
            xp = 0;
            Debug.Log("Error: Rarity int out of range. Xp for next level set to 0.");
        }

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

        string locked;
        if (isLocked)
        {
            locked = "1";
        }
        else
        {
            locked = "0";
        }

        ID = scrObj.TypeID.ToString() + "_" + currentLevel.ToString() + "_" + currentMaxLevel.ToString() + "_" + ascensionLevel.ToString() + "_" + currentXp.ToString() + "_" + locked;

        return ID;
    }
}
