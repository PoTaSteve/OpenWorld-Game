using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInfo : MonoBehaviour
{
    public MaterialScriptableObject MaterialSO;

    public int count;
    
    public string StringMaterialType()
    {
        string type;

        if (MaterialSO.materialType == MaterialTypeEnum.CrafingIngredient)
        {
            type = "Ingredient";
        }
        else if (MaterialSO.materialType == MaterialTypeEnum.CrafingResult)
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
