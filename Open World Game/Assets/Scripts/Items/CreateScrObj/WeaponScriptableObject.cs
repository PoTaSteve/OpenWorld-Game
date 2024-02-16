using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public string itemID;

    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;

    public Sprite itemIcon;

    public float baseAtk;

    public int buyCost;
    public int sellCost;

    public GameObject equipPrefab;
}
