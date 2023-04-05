using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodInfo : Interactable
{
    public FoodScriptableObject scrObj;

    public int count;

    public bool hasBottomRightValue;
    public bool hasTopRightValue;
    public bool hasTopLeftValue;

    public override void Interact()
    {
        GameManager.Instance.invMan.AddItemToInventoryST(ItemType.Food, gameObject);

        Destroy(gameObject);
    }
}
