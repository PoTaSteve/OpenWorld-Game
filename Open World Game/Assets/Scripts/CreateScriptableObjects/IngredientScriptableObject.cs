using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType
{
    Base,
    Specific
}

public enum IngrSpecificType
{
    Attack,
    Defence,
    StaminaConsumption,
    Speed,
    Heal,
    Time
}

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class IngredientScriptableObject : ScriptableObject
{
    //Top

    public string ingredientName;
    public IngredientType ingredientType;
    public int rarity;

    public Sprite icon;

    //Middle

    public bool isBase; // Can be base or specific
    public int baseHeal;

    public IngrSpecificType specificType;

    [TextArea(3, 10)]
    public string description;
    public int numberOfSources;
    public string[] sources;

    // Other
    public int cost;

    public string Console_Name;
    public int itemID;

    public string IngredientTypeEnumToString(IngredientType type)
    {
        string s;

        if (type == IngredientType.Base)
        {
            s = "Base Ingredient";
        }
        else
        {
            s = "Specific Ingredient";
        }

        return s;
    }

    public string IngrSpecificTypeEnumToString(IngrSpecificType type)
    {
        string s;

        if (type == IngrSpecificType.Attack)
        {
            s = "Attack";
        }
        else if (type == IngrSpecificType.Defence)
        {
            s = "Defence";
        }
        else if (type == IngrSpecificType.Heal)
        {
            s = "Heal";
        }
        else if (type == IngrSpecificType.Speed)
        {
            s = "Speed";
        }
        else if (type == IngrSpecificType.StaminaConsumption)
        {
            s = "Stamina Consumption";
        }
        else
        {
            s = "Time";
        }

        return s;
    }
}
