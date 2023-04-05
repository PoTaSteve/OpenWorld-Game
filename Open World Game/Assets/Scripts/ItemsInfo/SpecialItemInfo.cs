using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialItemInfo : Interactable
{
    public SpecialItemScriptableObject scrObj;

    public int count;

    public bool hasBottomRightValue;
    public bool hasTopRightValue;
    public bool hasTopLeftValue;

    public override void Interact()
    {
        GameManager.Instance.invMan.AddItemToInventoryST(ItemType.SpecialItem, gameObject);

        Destroy(gameObject);
    }
}
