using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Food,
    Potion
}

public enum FoodBuffType
{
    Attack,
    Defence,
    StaminaConsumption,
    Speed,
    Regen,
    Heal
}

[CreateAssetMenu(fileName = "New Food", menuName = "Food")]
public class FoodScriptableObject : ScriptableObject
{
    //Top

    public string foodName;
    public FoodType foodType;
    public int rarity;
    public FoodBuffType buffType;

    public Sprite icon;

    //Middle

    public bool isFood; // Food or potion
    [TextArea(3, 10)]
    public string effect;
    [TextArea(3, 10)]
    public string description;
    public int numberOfSources;
    public string[] sources;

    // Other
    public int cost;

    public string FoodTypeEnumToString(FoodType type)
    {
        string s;

        if (type == FoodType.Food)
        {
            s = "Food";
        }
        else if (type == FoodType.Potion)
        {
            s = "Potion";
        }
        else
        {
            s = "Error";
        }

        return s;
    }

    public string FoodBuffTypeEnumToString(FoodBuffType type)
    {
        string s;

        if (type == FoodBuffType.Attack)
        {
            s = "Attack";
        }
        else if (type == FoodBuffType.Defence)
        {
            s = "Defence";
        }
        else if (type == FoodBuffType.Heal)
        {
            s = "Heal";
        }
        else if (type == FoodBuffType.Speed)
        {
            s = "Speed";
        }
        else if (type == FoodBuffType.Regen)
        {
            s = "Regen";
        }
        else if (type == FoodBuffType.StaminaConsumption)
        {
            s = "Stamina Consumption";
        }
        else
        {
            s = "Error";
        }

        return s;
    }
}
