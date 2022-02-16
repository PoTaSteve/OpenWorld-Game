using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopProduct : MonoBehaviour
{
    public void OnProductSelected()
    {
        if (gameObject.TryGetComponent<MaterialInfo>(out MaterialInfo matInfo))
        {

        }
        else if (gameObject.TryGetComponent<IngredientInfo>(out IngredientInfo ingrInfo))
        {

        }
        else // FoodInfo
        {

        }

        // Update details
        // Update script in purchase button
    }
}
