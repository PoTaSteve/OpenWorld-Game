using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInfo : EquipableItem
{
    public EquipmentScriptableObject scrObj;

    public EquipmentInfo(EquipmentScriptableObject scrObj)
    {
        this.scrObj = scrObj;
    }

    public override int GetBuyCost()
    {
        return scrObj.buyCost;
    }

    public override int GetSellCost()
    {
        return scrObj.sellCost;
    }

    public override int GetItemIntType()
    {
        return (int)scrObj.equipType;
    }

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

    public override EquipType GetEquipType()
    {
        return scrObj.equipType;
    }

    public override void Equip()
    {

    }

    public override void Unequip()
    {

    }
}
