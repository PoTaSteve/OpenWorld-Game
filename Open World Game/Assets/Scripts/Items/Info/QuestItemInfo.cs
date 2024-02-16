using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestItemInfo : ItemInfo
{
    public QuestItemScriptableObject scrObj;

    public int count;

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
        return (int)ItemType.Quest_Item;
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
        GameManager.Instance.invMan.AddItemToInventory(this, count);

        GameManager.Instance.plInteractMan.InRangeInteractables.Remove(gameObject);

        Destroy(gameObject);
    }
}
