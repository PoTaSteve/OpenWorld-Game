using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Transform itemsParent;

    public List<WeaponScriptableObject> AllWeapons = new List<WeaponScriptableObject>();
    public List<MaterialScriptableObject> AllMaterials = new List<MaterialScriptableObject>();
    public List<IngredientScriptableObject> AllIngredients = new List<IngredientScriptableObject>();
    public List<FoodScriptableObject> AllFood = new List<FoodScriptableObject>();
    public List<SpecialItemScriptableObject> AllSpecialItem = new List<SpecialItemScriptableObject>();
}
