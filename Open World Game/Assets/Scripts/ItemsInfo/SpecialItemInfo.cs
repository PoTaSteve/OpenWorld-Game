using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialItemInfo : Interactable
{
    public SpecialItemScriptableObject scrObj;

    public int count;

    public override void Interact()
    {
        GameManager.Instance.invMan.AddItemToInventory(ItemType.SpecialItem, gameObject, count);

        GameManager.Instance.plInteractMan.InRangeInteractables.Remove(gameObject);

        Destroy(gameObject);
    }
}
