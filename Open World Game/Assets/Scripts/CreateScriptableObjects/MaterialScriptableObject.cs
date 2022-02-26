using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialTypeEnum
{
    CrafingIngredient,
    CrafingResult
}

[CreateAssetMenu(fileName = "New Material", menuName = "Material Item")]
public class MaterialScriptableObject : ScriptableObject
{
    //Top

    public string materialName;
    public MaterialTypeEnum materialType;
    public int rarity;

    public Sprite icon;

    //Middle

    public bool isCraftingResult;

    [TextArea(3, 10)]
    public string description;
    public int numberOfSources;
    public string[] sources;

    // Other
    public int cost;

    public string Console_Name;
    public int itemID;

    public string MaterialTypeEnumToString(MaterialTypeEnum type)
    {
        string s;

        if (type == MaterialTypeEnum.CrafingIngredient)
        {
            s = "Crafting Ingredient";
        }
        else
        {
            s = "Crafting Result";
        }

        return s;
    }
}
