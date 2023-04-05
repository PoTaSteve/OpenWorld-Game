using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialTypeEnum
{
    CrafingIngredient,
    CrafingResult,
    EnhanceMaterial
}

[CreateAssetMenu(fileName = "New Material", menuName = "Material Item")]
public class MaterialScriptableObject : ScriptableObject
{
    public int ItemID;

    //Top

    public string materialName;
    public MaterialTypeEnum materialType;

    public Sprite icon;

    //Middle

    public bool isCraftingResult;

    [TextArea(3, 10)]
    public string description;

    // Other
    public int buyCost;
    public int sellCost;

    public string Console_Name;
    public int enhanceXp;

    public string MaterialTypeEnumToString(MaterialTypeEnum type)
    {
        string s;

        if (type == MaterialTypeEnum.CrafingIngredient)
        {
            s = "Crafting Ingredient";
        }
        else if (type == MaterialTypeEnum.CrafingResult)
        {
            s = "Crafting Result";
        }
        else
        {
            s = "Enhance Material";
        }

        return s;
    }
}
