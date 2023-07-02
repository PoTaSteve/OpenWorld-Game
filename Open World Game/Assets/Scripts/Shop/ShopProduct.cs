using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopProduct : MonoBehaviour
{
    //public int startCount;
    //public int currCount;
    public int index;
    public int cost;

    public Sprite icon;
    public string itemName;
    public string description;

    public int rarity;

    public ItemType itemType;

    public WeaponInfo weapInfo;
    public MaterialInfo matInfo;
    public FoodInfo foodInfo;
    public SpecialItemInfo specItemInfo;
}
