using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInfo : MonoBehaviour
{
    public IngredientScriptableObject IngredientSO;

    public int count;

    public string StringIngredientType()
    {
        string type;

        if (IngredientSO.ingredientType == IngredientType.Base)
        {
            type = "Base";
        }
        else if (IngredientSO.ingredientType == IngredientType.Specific)
        {
            type = "Specific";
        }
        else
        {
            type = "Error";
        }

        return type;
    }
}
