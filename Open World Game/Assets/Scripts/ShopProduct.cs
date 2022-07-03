using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopProduct : MonoBehaviour
{
    public int startCount;
    public int currCount;
    public int cost;
    public int index;

    public Sprite icon;
    public string itemName;
    public string description;

    public int rarity;

    public ItemType itemType;

    public WeaponInfo weapInfo;
    public MaterialInfo matInfo;
    public IngredientInfo ingrInfo;
    public FoodInfo foodInfo;
    public SpecialItemInfo specItemInfo;
}
