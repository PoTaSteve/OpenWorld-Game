using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialInfo : Interactable
{
    public MaterialScriptableObject scrObj;

    public int count;

    public bool hasBottomRightValue;
    public bool hasTopRightValue;
    public bool hasTopLeftValue;

    public override void Interact()
    {
        GameManager.Instance.invMan.AddItemToInventoryST(ItemType.Material, gameObject);

        Destroy(gameObject);
    }

    public string StringMaterialType()
    {
        string type;

        if (scrObj.materialType == MaterialTypeEnum.CrafingIngredient)
        {
            type = "Ingredient";
        }
        else if (scrObj.materialType == MaterialTypeEnum.CrafingResult)
        {
            type = "Result";
        }
        else
        {
            type = "Error";
        }

        return type;
    }
}
