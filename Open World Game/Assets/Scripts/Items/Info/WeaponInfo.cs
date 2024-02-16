using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfo : EquipableItem
{
    public WeaponScriptableObject scrObj;

    public override string GetDescription()
    {
        return scrObj.itemDescription;
    }

    public override Sprite GetIcon()
    {
        return scrObj.itemIcon;
    }

    public override string GetID()
    {
        return scrObj.itemID;
    }

    public override string GetName()
    {
        return scrObj.itemName;
    }

    public override void Interact()
    {
        GameManager.Instance.equipmentMan.AddItemToEquipmentInventory(this);

        GameManager.Instance.plInteractMan.InRangeInteractables.Remove(gameObject);

        Destroy(gameObject);
    }

    public override int GetItemIntType()
    {
        return (int)EquipType.Weapon;
    }

    public override int GetBuyCost()
    {
        return scrObj.buyCost;
    }

    public override int GetSellCost()
    {
        return scrObj.sellCost;
    }

    public override EquipType GetEquipType()
    {
        return EquipType.Weapon;
    }

    public override void Equip()
    {
        
    }

    public override void Unequip()
    {
        
    }
}
