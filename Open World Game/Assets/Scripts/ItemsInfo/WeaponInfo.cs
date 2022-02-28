using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfo : Interactable
{
    public WeaponScriptableObject SO;

    #region InventoryInfos
    //Top

    public float baseATK;

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
        WeaponInfo newSlot = Instantiate<WeaponInfo>(InventoryManager.Instance.WeaponInvSlot.GetComponent<WeaponInfo>(), InventoryManager.Instance.TabsContent[0]);
        InventoryManager.Instance.WeaponsTab.Add(newSlot);
        InventoryManager.Instance.WeaponTabStr.Add(SO.weaponName);
        newSlot.GetComponent<Button>().onClick.AddListener(InventoryManager.Instance.UpdateWeaponInvSlotDetails);

        newSlot.SO = SO;

        SetAtkFromLevel();
        newSlot.baseATK = baseATK;

        newSlot.currentXp = currentXp;
        xpForNextLevel = XpForNextLevel(currentLevel);
        newSlot.xpForNextLevel = xpForNextLevel;

        newSlot.currentLevel = currentLevel;
        newSlot.currentMaxLevel = currentMaxLevel;
        newSlot.ascensionLevel = ascensionLevel;
        newSlot.isLocked = isLocked;

        // Instantiate Slot: Slot UI
        newSlot.transform.GetChild(2).gameObject.SetActive(false);
        newSlot.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Lv. " + newSlot.currentLevel;
        newSlot.transform.GetChild(1).GetComponent<Image>().sprite = newSlot.SO.icon;

        Transform Rarity = newSlot.transform.GetChild(3);
        foreach (Transform t in Rarity)
        {
            t.gameObject.SetActive(false);
        }
        for (int i = 0; i < newSlot.SO.rarity; i++)
        {
            Rarity.GetChild(i).gameObject.SetActive(true);
        }

        if (newSlot.isLocked)
        {
            newSlot.transform.GetChild(5).gameObject.SetActive(true);
            newSlot.transform.GetChild(5).GetChild(0).GetComponent<Image>().color = InventoryManager.Instance.closedPadlockColBG;
            newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().sprite = InventoryManager.Instance.PadlockClosed;
            newSlot.transform.GetChild(5).GetChild(2).GetComponent<Image>().color = InventoryManager.Instance.closedPadlockCol;
        }
        else
        {
            newSlot.transform.GetChild(5).gameObject.SetActive(false);
        }

        newSlot.transform.GetChild(6).gameObject.SetActive(true);

        #endregion
    }

    public void Start()
    {
        SetAtkFromLevel();
        xpForNextLevel = XpForNextLevel(currentLevel);
    }

    public void SetAtkFromLevel()
    {
        if (currentLevel <= 20 && ascensionLevel == 0)
        {
            baseATK = SO.BaseATK1 + SO.incrementATK * (currentLevel - 1);
        }
        else if (currentLevel >= 20 && currentLevel <= 40 && ascensionLevel == 1)
        {
            baseATK = SO.BaseATK20 + SO.incrementATK * (currentLevel - 20);
        }
        else if (currentLevel >= 40 && currentLevel <= 60 && ascensionLevel == 2)
        {
            baseATK = SO.BaseATK40 + SO.incrementATK * (currentLevel - 40);
        }
        else if (currentLevel >= 60 && currentLevel <= 70 && ascensionLevel == 3)
        {
            baseATK = SO.BaseATK60 + SO.incrementATK * (currentLevel - 60);
        }
        else if (currentLevel >= 70 && currentLevel <= 80 && ascensionLevel == 4)
        {
            baseATK = SO.BaseATK70 + SO.incrementATK * (currentLevel - 70);
        }
        else if (currentLevel >= 80 && currentLevel <= 90 && ascensionLevel == 5)
        {
            baseATK = SO.BaseATK80 + SO.incrementATK * (currentLevel - 80);
        }
        else if (currentLevel >= 90 && currentLevel < 100 && ascensionLevel == 6)
        {
            baseATK = SO.BaseATK90 + SO.incrementATK * (currentLevel - 90);
        }
        else if (currentLevel == 100 && ascensionLevel == 6)
        {
            baseATK = SO.BaseATK100;
        }
    }

    public void SetSecondaryStatFromLevel()
    {
        // To implement
    }

    public int XpForNextLevel(int level) // Xp to get from a level to the next. Parameter level is the lower level
    {
        int xp = (int)(Mathf.Pow((5f / 2f) * level + 5, 2f) + 100); // Common
        //int xp2 = (int)(Mathf.Pow((14f / 5f) * level + 5, 2f) + 150); // Uncommon
        // Hypotesis of integral

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

    public string StringWeaponType()
    {
        string type;

        if (SO.weaponType == WeaponType.Sword)
        {
            type = "Sword";
        }
        else if (SO.weaponType == WeaponType.Bow)
        {
            type = "Bow";
        }
        else if (SO.weaponType == WeaponType.Claymore)
        {
            type = "Claymore";
        }
        else if (SO.weaponType == WeaponType.Staff)
        {
            type = "Staff";
        }
        else if (SO.weaponType == WeaponType.Polearm)
        {
            type = "Polearm";
        }
        else
        {
            type = "Error";
        }

        return type;
    }
}
