using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Valuable Item", menuName = "Item/Valuable Item")]
public class ValuableItemScriptableObject : ScriptableObject
{
    public string itemID;

    public string itemName;
    public string itemDescription;

    public Sprite itemIcon;

    public int buyCost;
    public int sellCost;
}
