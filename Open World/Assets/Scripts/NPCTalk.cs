using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType
{
    Chatty,
    Intractive
}

public class NPCTalk : MonoBehaviour
{
    public TextAsset inkJSONAsset;
    public NPCType type;

    [Header("Shop Products")]
    public List<MaterialInfo> Materials;
    public List<IngredientInfo> Ingredients;
    public List<FoodInfo> Food;
    [Space]
    [Header("Products Count")]
    public List<int> materialCount;
    public List<int> ingredientCount;
    public List<int> foodCount;
    [Space]
    [Header("Products Max Count")]
    public List<int> materialMaxCount;
    public List<int> ingredientMaxCount;
    public List<int> foodMaxCount;
}
